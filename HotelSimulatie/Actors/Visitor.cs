using HotelSimulatie.Actors;
using HotelSimulatie.Facilities;
using HotelSimulatie.Enums;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using HotelSimulatie.Factory;
using HotelSimulatie.Graph;
using HotelEvents;
using System.Text.RegularExpressions;
using System.Diagnostics;
using System.Timers;
using HotelSimulatie.Model;

namespace HotelSimulatie.Actors
{
    public class Visitor : Human
    {
        // Visitor has got a room
        public Room Room { get; private set; }
        // How there can be communicated with visitor
        public InteractStatus InteractStatus { get; private set; } = InteractStatus.NONE;
        // status of a visitor eating
        public EatingStatus EatingStatus { get; private set; } = EatingStatus.NONE;


        /// <summary>
        /// constructor where we will initialise values
        /// </summary>
        /// <param name="humanParam"></param>
        public Visitor(MovableParam humanParam) : base(humanParam)
        {
            // Set properties
            Path = new List<Node>();
            Image = Image.FromFile("../../Resources/lemming.png");
            StatusHuman = StatusHuman.NONE;
            ID = humanParam.ID;
        }


        /// <summary>
        /// Updating visitor for the reason gameloop
        /// </summary>
        /// <param name="drawUpdateTime"></param>
        public override void Update(double drawUpdateTime)
        {        
            // Check for walking status
            WalkCheck(drawUpdateTime);
            // Check if visitor is going to interact
            InteractWithDestination(drawUpdateTime);
        }

        /// <summary>
        /// interact with destination
        /// </summary>
        /// <param name="drawUpdateTime"></param>
        public void InteractWithDestination(double drawUpdateTime)
        {
            // When the visitor is in a facility it interacts
            if (StatusHuman == StatusHuman.INTERACTING)
            {
                // Visitor decides when he can make a path back. This we do by taking the doingEvent value minus the HTE we get by loading a file.
                if (CurrentEvent.EventType == HotelEventType.GOTO_FITNESS)
                {
                    foreach (var item in CurrentEvent.Data)
                    {
                        // When we send the event data from fitness to visitor, it has two dictionaries. We need the one where the key is "HTE".
                        if (item.Key == "HTE")
                        {
                            // Visitor is fitnessing
                            // Dictract from OneHte, HTE we get from the DLL file, when it is zero, visitor will leave the fitness centrum.
                            OneHTE -= drawUpdateTime / (Convert.ToInt32(item.Value) / Settings.Hte);
                        }
                    }
                }
                // Visitor decides when he can make a path back. This we do by taking the doingEvent value minus the HTE we get by loading a file.
                else if (CurrentEvent.EventType == HotelEventType.NEED_FOOD)
                {
                    if (EatingStatus == EatingStatus.NONE)
                    {
                        // Visitor is eating
                        //Console.WriteLine("eating");
                        // Dictract from doingEvent value the HTE we get from the DLL file, when it is zero, visitor will leave the restaurant.
                        OneHTE -= drawUpdateTime / Settings.RestaurantTimeUnit;
                    }
                    else if (EatingStatus == EatingStatus.POISONED)
                    {
                        // call : BurnRestaurantDown method from visitor class
                    }
                    else if (EatingStatus == EatingStatus.DONT_LIKE_FOOD)
                    {
                        // call : PayMoneyBack() method from restaurant class
                    }
                }

                if (OneHTE < 1)
                {
                    // Visitor is done
                    //Console.WriteLine("Im done eating or fitnessing");
                    // When the count is on zero, we let the visitor leave the facility.
                    this.Location.LocationType.LeaveFacility(this);
                    // We set his current event on NONE, so we know when the visitor wants to move from facility to another one instead of returning back. When visitor leaves we set in on NONE
                    CurrentEvent.EventType = HotelEventType.NONE;
                    // We let him make a path to his room.
                    PathToLocationType();
                    // We set the current event on NONE at this moment, because we dont want to send a visitor to a facility with NONE value. He cant interact then.
                    OneHTE = 1000;
                }
            }
            
            // Facilities talk with visitors by telling them that they are done being helped, so visitor can make a path back to room.
            if (InteractStatus == InteractStatus.DONE_BEING_HELPED)
            {
                // We set the current event on NONE at this moment, because we dont want to send a visitor to a facility with NONE value. He cant interact then. When he leaves we set it on NONE
                CurrentEvent.EventType = HotelEventType.NONE;
                // Make a path back to room
                PathToLocationType();
                // Set visitor's interact status on NONE so this method won't be repeated all the time.
                InteractStatus = InteractStatus.NONE;
            }
        }

        /// <summary>
        /// How facilities communicate with visitor
        /// </summary>
        /// <param name="rememberTalking"></param>
        public void Communicate(InteractStatus rememberTalking)
        {
            InteractStatus = rememberTalking;
        }

        /// <summary>
        /// afther event comes in
        /// </summary>
        public override void SetWalkingStatus()
        {
            // If we get the evacuate notify, we make the status of a visitor evacuate, so later on, we can make good use of this for not letting visitor leave his path at this circumstance.
            if (CurrentEvent.EventType == HotelEventType.EVACUATE)
            {
                StatusHuman = StatusHuman.EVACUATE;
            }
            // Visitor will walk when status is walk
            else
            {
                if (!(Location.LocationType is ElevatorShaft))
                {
                    StatusHuman = StatusHuman.WALK;
                }
            }
        }

        /// <summary>
        /// the path we want the visitor to follow
        /// </summary>
        public void PathToLocationType()
        {
            if(StatusHuman != StatusHuman.WAITING || StatusHuman == StatusHuman.WAITING && CurrentEvent.EventType == HotelEventType.EVACUATE)
            {
                // We want visitor to be alive when creating a path
                if (Alive == true)
                {
                    // To set the first location
                    if (this.Location.LocationType == null)
                    {
                        foreach (var facility in PathFinding.facilities)
                        {
                            if (facility.Position.X * facility.oneDimensionSize.Width == this.Xpos && facility.Position.Y * facility.oneDimensionSize.Height == this.Ypos)
                            {
                                this.Location.SetLocationType(facility);
                            }
                        }
                    }

                    // When visitor his location is his room, he has to interact to leave it
                    if (this.Location.LocationType == this.Room)
                    {
                        this.Location.LocationType.Interact(this);
                    }

                    // If the visitor it's status is not evacuate we make a path to his wanted destination, or else he will keep walking to the destination of the evacuation
                    if (StatusHuman != StatusHuman.EVACUATE)
                    {
                        // We make a list pf paths we can later on choose which path is the fastest to walk
                        List<List<Node>> pathList = new List<List<Node>>();
                        // if visitor is in room > go to facility
                        // if firststep is true > to to facility
                        // if location is not destination && not room > go to facility
                        // if location is destination && current event is not none > go to facility
                        // if current event is evacuate > go to facility
                        // if (this.Location.LocationType == this.Room || CurrentEvent.EventType == HotelEventType.CHECK_IN || this.Location.LocationType != this.Destination && this.Location.LocationType != this.Room || this.Location.LocationType == Destination && CurrentEvent.EventType != HotelEventType.NONE || CurrentEvent.EventType == HotelEventType.EVACUATE)

                        if (CurrentEvent.EventType != HotelEventType.NONE)
                        {
                            // based on the current event we return a facility
                            foreach (var item in SearchFacility(CurrentEvent.EventType))
                            {
                                // create a new graph
                                Graph.Graph graph = new Graph.Graph();
                                // add those pahts to the multi Dimensional list
                                pathList.Add(graph.CreateNodeGraph(Location.LocationType, item, PathFinding, CurrentEvent.EventType));
                            }
                            // get the fastest path
                            TakeClosestFacility(pathList);
                        }
                        else
                        {
                            Graph.Graph graph = new Graph.Graph();
                            Path = graph.CreateNodeGraph(Location.LocationType, this.Room, PathFinding, CurrentEvent.EventType);
                        }
                        // We use this to determin if visitor is on his destination
                        Destination = Path.Last().LocationType;

                        LocationType firstElement = Path.First().LocationType;
                        if (firstElement is ElevatorShaft && CurrentEvent.EventType != HotelEventType.EVACUATE)
                        {
                            NextStep = 0;
                        }
                        else
                        {
                            // if visitor changes his mind, he's steps will be reset. Step 0 is his location where he stands on, we don't want him to walk to that place
                            NextStep = 1;
                        }

                        // Here we let the visitor walk again
                        SetWalkingStatus();
                    }
                }   
            }
        }

        /// <summary>
        /// Take the closest facility visitor wants to walk to
        /// </summary>
        /// <param name="pathLists"></param>
        public void TakeClosestFacility(List<List<Node>> pathLists)
        {
            // create a list
            List<Node> currentPath = null;
            // search the paths
            foreach (var item in pathLists)
            {
                // if it's not null
                if (currentPath != null)
                {
                    // see if it's smaller then current
                    if (item.Count < currentPath.Count)
                    {
                        // set is as the new path
                        currentPath = item;
                    }
                }
                else
                {
                    // set path
                    currentPath = item;
                }
            }
            // We set the path on the fastest path to destination
            Path = currentPath;
        }

        /// <summary>
        /// Find destination
        /// </summary>
        /// <param name="typeMoveEvent"></param>
        /// <returns></returns>
        public List<LocationType> SearchFacility(HotelEventType typeMoveEvent)
        {
            // where we add the facilities where visitor wants to go to
            List<LocationType> amountFacilities = new List<LocationType>();
            // Reurn based on the event a instance
            foreach (var item in PathFinding.facilities)
            {
                // switch based on events
                switch (typeMoveEvent)
                {
                    // return a lobby
                    case HotelEventType.CHECK_OUT:
                    case HotelEventType.CHECK_IN:
                    case HotelEventType.EVACUATE:
                        if (item is Lobby)
                            {
                                amountFacilities.Add(item);
                            }
                        break;
                        // return the fitness-halls
                    case HotelEventType.GOTO_FITNESS:
                            if (item is Fitness)
                            {
                                amountFacilities.Add(item);
                            }
                        break;
                        // return the cinemas
                    case HotelEventType.GOTO_CINEMA:
                    case HotelEventType.START_CINEMA:
                        if (item is Cinema)
                            {
                                amountFacilities.Add(item);
                            }
                        break;
                        // return the restaurants
                    case HotelEventType.NEED_FOOD:
                            if (item is Restaurant)
                            {
                                amountFacilities.Add(item);
                            }
                        break;
                    default:
                        break;
                }
            }
            // return list
            return amountFacilities;
        }

        /// <summary>
        /// remembers room himself, thats why it's private
        /// </summary>
        /// <param name="ownRoom"></param>
        public void GetRoom(Room ownRoom)
        {
            Room = ownRoom;
        }
    }
}
