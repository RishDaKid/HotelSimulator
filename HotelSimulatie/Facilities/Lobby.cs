using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using HotelSimulatie.Actors;
using HotelSimulatie.Graph;
using System.Timers;
using System.IO;
using Newtonsoft.Json;
using HotelSimulatie.Manage;
using System.Collections.ObjectModel;
using HotelSimulatie.Factory;
using HotelSimulatie.Enums;

namespace HotelSimulatie.Facilities
{
    public class Lobby : LocationType
    {
        // Lobby got the list of Rooms because lobby will manage
        public List<Room> Rooms { get; private set; }
        // List of Rooms which has to be cleaned
        public List<Room> DirtyRooms { get; private set; }
        // List of Cleaners where we will place the created cleaners in
        public List<Cleaner> Cleaners { get; private set; }
        // List of people who are being evacuated
        public List<Human> EvacuatedPeople { get; private set; }
        // List of visitors we have here because lobby will manage and create them
        public List<Visitor> Visitors { get; private set; }

        public Lobby(Facility specs) : base(specs)
        {
            Image = Image.FromFile("../../Resources/Lobbyy.png");
            Visitors = new List<Visitor>();
            Rooms = new List<Room>();
            DirtyRooms = new List<Room>();
            Cleaners = new List<Cleaner>();
            EvacuatedPeople = new List<Human>();
        }


        /// <summary>
        /// Updating visitors and their walkCheck
        /// Checking queue, evacuation and dirtyrooms
        /// </summary>
        /// <param name="drawUpdateTime"></param>
        public override void Update(double drawUpdateTime)
        {
            // Check entrace queue
            CheckQueue(drawUpdateTime);
            // Check if people has to be evacuated
            EvacuatePeopleCheck(drawUpdateTime);
            // Check if there are rooms that need to be cleaned
            CheckDirtyRooms();

            // Update every visitor
            for (int visitor = 0; visitor < Visitors.Count; visitor++)
            {
                if(Visitors[visitor].Alive == true)
                    Visitors[visitor].Update(drawUpdateTime);
            }

            // Update every cleaner
            for (int cleaner = 0; cleaner < Cleaners.Count; cleaner++)
            {
                if (Cleaners[cleaner].Alive == true)
                    Cleaners[cleaner].Update(drawUpdateTime);
            }
        }

        /// <summary>
        /// interacting with visitor based on the reason he interacts with lobby
        /// </summary>
        /// <param name="human"></param>
        public override void Interact(Human human)
        {
            if (human is Visitor)
            {
                if (human.CurrentEvent.EventType == HotelEvents.HotelEventType.EVACUATE)
                {
                    EvacuatePeople(human);
                }
                if (human.CurrentEvent.EventType == HotelEvents.HotelEventType.CHECK_OUT)
                {
                    CheckVisitorOut(human);
                }
                if (human.CurrentEvent.EventType == HotelEvents.HotelEventType.CHECK_IN)
                {
                    Enqeue(human);
                }
            }
            else
            {
                if (human.CurrentEvent.EventType == HotelEvents.HotelEventType.EVACUATE)
                {
                    EvacuatePeople(human);
                }
                else
                {
                    JoinFacility(human as Cleaner);
                }
            }
        }

        /// <summary>
        /// Checking if people are being evacuated. If all of visitors are evacuated, they are released into the hotel again
        /// </summary>
        /// <param name="drawUpdateTime"></param>
        public void EvacuatePeopleCheck(double drawUpdateTime)
        {
            // When all visitors have gathered
            if (EvacuatedPeople.Count == Visitors.Count + Cleaners.Count)
            {
                // Substract untill integer becomes zero
                OneHTE -= (int)drawUpdateTime / HTE.Evacuation;
                if (OneHTE < 1)
                {
                    // Tell visitor that we are done helping him so he can walk away
                    for (int i = 0; i < EvacuatedPeople.Count; i++)
                    {
                        if(EvacuatedPeople[i] is Visitor)
                        (EvacuatedPeople[i] as Visitor).Communicate(InteractStatus.DONE_BEING_HELPED);
                    }
                    for (int i = 0; i < EvacuatedPeople.Count; i++)
                    {
                        EvacuatedPeople.Remove(EvacuatedPeople[i]);
                    }
                    OneHTE = 1000;
                }
            }
        }

        /// <summary>
        /// Lobby is checking of there is a room to be cleaned. If so, the lobby will ask a cleaner to clean it
        /// </summary>
        public void CheckDirtyRooms()
        {
            // when there is a room to clean
            if (DirtyRooms.Count != 0)
            {
                // Loop in cleaners
                foreach (var item in Cleaners)
                {
                    for (int specificRoom = 0; specificRoom < DirtyRooms.Count; specificRoom++)
                    {
                        // if his status goingtowork is false
                        if (item.StatusCleaner == StatusCleaner.NOT_WORKING && item.Alive == true && item.StatusHuman != StatusHuman.EVACUATE)
                        {
                            ManageCleaner manageCleaner = new ManageCleaner();
                            manageCleaner.LetWorkerClean(DirtyRooms[specificRoom], Cleaners);
                            DirtyRooms.Remove(DirtyRooms[specificRoom]);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// When there is someone in the entrance queue, we count down based on the time the room helps the incoming visitor
        /// </summary>
        /// <param name="drawUpdateTime"></param>
        public void CheckQueue(double drawUpdateTime)
        {
            // if the entrance queue isn't empty
            if (EntranceQueue.Count != 0)
            {
                // substract untill it becomes 0
                OneHTE -= ((int)((drawUpdateTime))) / (HTE.Queue) * 2; 
                if (OneHTE < 1)
                {
                    // take and remove visitor out of list
                    Human currentVisitor = Dequeue();
                    // Check him in
                    CheckIn(currentVisitor);
                    // Reset the timer again for the following visitors
                    OneHTE = 1000;
                }
            }
        }

        /// <summary>
        /// Lobby gets a notification what room has to be placed in the dirtyroom list.
        /// </summary>
        /// <param name="evtData"></param>
        public void AddDirtyRoom(Dictionary<string, string> evtData)
        {
            foreach (KeyValuePair<string, string> item in evtData)
            {
                // if the dictionary it's key is "kamer"
                if (item.Key == "kamer")
                {
                    // loop in rooms
                    foreach (var room in Rooms)
                    {
                        // Where room it's id is the same as the given id from the dicionary
                        if (room.ID.ToString() == item.Value)
                        {
                            // The room is dirty and has to be cleaned
                            room.CleaningStatus(false);
                            // Add the room to the to be cleaned list
                            DirtyRooms.Add(room);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Get first visitor out of the list to check him in
        /// </summary>
        /// <returns></returns>
        public Human Dequeue()
        {
            return EntranceQueue.Dequeue();
        }

        /// <summary>
        /// Visitor joins the queue to checkin
        /// </summary>
        /// <param name="human"></param>
        public void Enqeue(Human human)
        {
            EntranceQueue.Enqueue(human);
        }

        /// <summary>
        /// We are searching for a visitor based on the given rate. Afther that, we register him.
        /// </summary>
        /// <param name="interactingVisitor"></param>
        /// <returns></returns>
        public Visitor CheckIn(Human interactingVisitor)
        {
            Visitor visitor = interactingVisitor as Visitor;
            foreach (var info in visitor.CurrentEvent.Data)
            {
                int wantedroomClassification = 0; // what rate room a visitor wants
                // take rate of room the visitor want
                for (int i = 0; i < info.Value.Length; i++)
                {
                    if (Char.IsDigit(info.Value[i]))
                    {
                        wantedroomClassification += (int)char.GetNumericValue(info.Value[i]);
                        break;
                    }
                }

                // check if room is not empty, then take the rate of that room, check if that rate equals the rate the person want
                foreach (var specificRoom in Rooms)
                {
                    int roomClassification = 0; // what rate of a room

                    // check if the room is empty
                    if (specificRoom.Empty == true) // if room is empty
                    {
                        for (int i = 0; i < specificRoom.Classification.Length; i++)
                        {
                            if (Char.IsDigit(specificRoom.Classification[i]))
                            {
                                roomClassification += (int)char.GetNumericValue(specificRoom.Classification[i]);
                                break;
                            }
                        }
                        if (roomClassification == wantedroomClassification)
                        {
                            // Create Visitor and fill information                                   
                            // Visitor sets it's own because we don't want anyone else to do it. 
                            visitor.GetRoom(specificRoom);
                            // We now say that his room is not empty so other people cant hire that specific room.
                            specificRoom.Empty = false;
                            // Tell visitor he is done being helped so visitor can set his status on what he wants
                            visitor.Communicate(InteractStatus.DONE_BEING_HELPED);
                            // add visitor to list
                            break;
                        }
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Check visitor out by deleting it and set his room on : to be cleaned.
        /// // Set status of room Available
        /// </summary>
        /// <param name="human"></param>
        public void CheckVisitorOut(Human human)
        {
            for (int i = 0; i < Visitors.Count; i++)
            {
                if (human == Visitors[i])
                {
                    // Room is dirty, so we set status on false
                    Visitors[i].Room.CleaningStatus(false);
                    // Add Visitor his room to dirytyroom list
                    DirtyRooms.Add(Visitors[i].Room);
                    // Set his room available to be hired again
                    Visitors[i].Room.Empty = true;
                    // Remove the visitor from the visitor list
                    Visitors.Remove(Visitors[i]);
                }
            }
        }

        /// <summary>
        /// Add people too the evacuation list
        /// </summary>
        /// <param name="human"></param>
        public void EvacuatePeople(Human human)
        {
            EvacuatedPeople.Add(human);
        }

        /// <summary>
        /// Search for visitor method based on the id of the dictionarry and the id of visitors
        /// </summary>
        /// <param name="vistorInfo"></param>
        /// <returns></returns>
        public Visitor SearchVisitor(Dictionary<string, string> vistorInfo)
        {
            foreach (KeyValuePair<string, string> info in vistorInfo)
            {
                // Search in visitor list
                foreach (var item in Visitors)
                {
                    if (info.Key != "HTE")
                    {
                        // if the id from visitor is the same as the id coming from the dictionary return visitor
                        if (item.ID.ToString() == info.Value)
                        {
                            return item;
                        }
                        else
                        {
                            // Get the id(s) from the dictionarry
                            string id = null;
                            for (int i = 0; i < info.Key.Length; i++)
                            {
                                if (Char.IsDigit(info.Key[i]))
                                {
                                    id += char.GetNumericValue(info.Key[i]);
                                }
                            }
                            // if it's the same ad the id from visitor return visitor
                            if (item.ID.ToString() == id)
                            {
                                return item;
                            }
                        }
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Lobby sets it's room himself
        /// </summary>
        /// <param name="_rooms"></param>
        public void GetRoom(Room room)
        {
            Rooms.Add(room);
        }
        
        /// <summary>
        /// Now lobby can get the cleaners which are made in the Hotel
        /// </summary>
        /// <param name="cleaners"></param>
        public void GetCleaners(List<Cleaner> cleaners)
        {
            Cleaners = cleaners;
        }

        /// <summary>
        /// Now lobby can get the visitors which are made in the Hotel
        /// </summary>
        /// <param name="visitor"></param>
        public void GetVisitor(Visitor visitor)
        {
            Visitors.Add(visitor);
        }
    }
}
