using HotelSimulatie.Actors;
using HotelSimulatie.Facilities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HotelSimulatie.Factory;
using System.Windows.Forms;
using HotelSimulatie.Graph;
using HotelEvents;
using System.Text.RegularExpressions;
using System.Diagnostics;
using System.Reflection;
using HotelSimulatie.Model;
using System.Dynamic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using HotelSimulatie.Monster;
using System.Drawing.Imaging;

// prop altijd hoofdletter
// enum achter de aanmaak initialiseren
// prop maken als je voor buiten beschikbaar wilt stellen / als variabel altijd private is
// parameter kleiner letter dan hoofdletter
// variabel kleine letter

namespace HotelSimulatie
{
    public class Hotel : HotelEventListener
    {
        // Hotel has an status
        public HotelEventType StatusHotel { get; private set; } = HotelEventType.NONE;
        // Hotel has a height
        public int HotelHeight { get; private set; }
        // Hotel has a width
        public int HotelWidth { get; private set; }
        // Hotel is made of Facilities
        public List<LocationType> Facilities { get; private set; }
        // This is who manages visitors and cleaners
        private Lobby lobby;
        // We want to know how long this programm has been running
        public Stopwatch HotelTimer { get; private set; }
        // One pathfinding every human can use
        private PathFinding pathfinding;
        // The one and only elevator we have
        private Elevator elevator;
        // Where we will store the HTE settingsfile
        private Settings hte;

        /// <summary>
        /// This is the constructor off the hotel
        /// </summary>
        /// <param name="file"></param>
        public Hotel(string file, Settings hte)
        {
            // Setting the settingsfile of this class
            this.hte = hte;

            // Definitions
            HotelTimer = new Stopwatch();
            Facilities = new List<LocationType>();

            // Methods
            CreateFactory();
            CreateFactoryObjects(file);
            LinkLocationTypes();
            CreateCleaners();

            // We add this class to the listener list so it can be updated
            HotelEventManager.Register(this);
            // Setting the HTE on how fast the events should come in
            HotelEventManager.HTE_Factor = hte.Hte;
            // Start the events
            HotelEventManager.Start();
            // Start the timer
            HotelTimer.Start();
        }

        /// <summary>
        /// Hotel is getting updated and is updating in it's updating method, the update methode of all locationtypes
        /// </summary>
        /// <param name="drawUpdateTime"></param>
        public void Update(double drawUpdateTime)
        {
            for (int i = 0; i < Facilities.Count; i++)
            {
                Facilities[i].Update(drawUpdateTime);
            }
            if(elevator != null)
            elevator.Update(drawUpdateTime);
        }

        /// <summary>
        /// Moster can interact with the Hotel based by it's type
        /// </summary>
        /// <param name="monster"></param>
        public void Interact(Monster.Monster monster)
        {
            if(monster is Godzilla)
            {
                DestroyLocationTypes();
                InitialiseHotelDimension();
            }
        }

        /// <summary>
        /// Removing locationtypes when there Dimension is a 0. You can see the Dimension of a locationtype has it's healthbar.
        /// We take the biggest most right collum of the hotel and send the position and Dimension to all locationtypes that get's notified
        /// Locationtypes compares those numbers to see if it's health(Dimension) has to be substracted
        /// When locationtype is being attacked and it's Dimension is at one, he will be deleted because every attacks removes one health(Dimension)
        /// </summary>
        public void DestroyLocationTypes()
        {
            // setting status on Godzilla so we can define how to interact by this
            StatusHotel = HotelEventType.GODZILLA;
            // When there are facilities, it will be attacked by the godzilla
            if (Facilities.Count != 0)
            {
                // Take the biggest locationtype by combining the xPosition and xDimension
                LocationType Biggest = Facilities.Aggregate((i1, i2) => (i1.Position.X + i1.Dimension.X) > (i2.Position.X + i2.Dimension.X) ? i1 : i2);
                // Visitor should die 
                foreach (var specificVisitor in lobby.Visitors)
                {
                    specificVisitor.Die();
                }
                // Cleaner should die
                foreach (var specificCleaner in lobby.Cleaners)
                {
                    specificCleaner.Die();
                }
                // Loop trought all facilities to notify them
                for (int facility = 0; facility < Facilities.Count; facility++)
                {
                    if (Facilities[facility].Notify(StatusHotel, Biggest.Position.X, Biggest.Dimension.X)==true)
                    {
                        // When locationtypes tells us that his health is at one, he should be deleted because there is no use to set it at zero
                        if (Facilities[facility] is ElevatorShaft)
                            elevator = null;
                        Facilities.Remove(Facilities[facility]);
                        facility--;
                    }
                }
                // When destroying is done, set status on NONE again
                StatusHotel = HotelEventType.NONE;
            }
        }

        /// <summary>
        /// Hotel is going to be notified because there can only be one item registered in the listener 
        /// Hotel will "notify" humans and locationtypes
        /// </summary>
        /// <param name="evt"></param>
        public void Notify(HotelEvent evt)
        {
            // Sometimes we have more destinations
            List<LocationType> destination = new List<LocationType>();
            // The visitor we have found from the list
            Visitor visitor = null;

            // We search on basis off the event enumeration
            switch (evt.EventType)
            {
                // When visitor wants to checkin
                case HotelEventType.CHECK_IN:
                    visitor = CreateVisitor(evt.Data);
                    // Giving visitor a location
                    visitor.GetCurrentEvent(evt);
                    // Visitor mag starten
                    visitor.PathToLocationType();
                    break;

                case HotelEventType.GOTO_CINEMA:
                case HotelEventType.GOTO_FITNESS:
                case HotelEventType.CHECK_OUT:
                    visitor = lobby.SearchVisitor(evt.Data);
                    if (visitor != null)
                    {
                        visitor.GetCurrentEvent(evt);
                        visitor.PathToLocationType();
                    }
                    break;

                // When cinema is going to start
                case HotelEventType.START_CINEMA:
                    List<LocationType> cinema = new List<LocationType>();
                    foreach (var item in Facilities)
                    {
                        if (item is Cinema)
                        {
                            cinema.Add(item);
                        }
                    }
                    foreach (var item in cinema)
                    {
                        foreach (var cinemaEvt in evt.Data)
                        {
                            if (cinemaEvt.Value == item.ID.ToString())
                            {
                                (item as Cinema).StartMovie();
                            }
                        }
                    }
                    break;

                // When someone wants to go to a restaurant to eat
                case HotelEventType.NEED_FOOD:
                    visitor = lobby.SearchVisitor(evt.Data);
                    if (visitor != null)
                    {
                        //Console.WriteLine("eten" + visitor.ID);

                        visitor.GetCurrentEvent(evt);
                        visitor.PathToLocationType();
                    }
                    break;

                // When there is an evacuation
                case HotelEventType.EVACUATE:
                    foreach (var item in lobby.Visitors)
                    {
                        item.GetCurrentEvent(evt);
                        item.PathToLocationType();
                    }
                    foreach (var item in lobby.Cleaners)
                    {
                        item.GetCurrentEvent(evt);
                        item.CreateGraph(lobby);
                        item.SetWalkingStatus();
                    }
                    elevator.EmergencyShutdown();
                    break;

                // When there is a room to be cleaned
                case HotelEventType.CLEANING_EMERGENCY:
                    lobby.AddDirtyRoom(evt.Data);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Hotel will handle the drawing on bitmap by returning the drawings to the mainscreen
        /// </summary>
        /// <param name="canvas"></param>
        /// <param name="worldBitmap"></param>
        public void DrawHotel(Graphics canvas, Bitmap worldBitmap)
        {
            // draw facilities
            for (int specificObject = 0; specificObject < Facilities.Count; specificObject++)
            {
                Facilities[specificObject].Draw(canvas, worldBitmap);
            }

            // Draw visitors
            for (int specificObject = 0; specificObject < lobby.Visitors.Count; specificObject++)
            {
                lobby.Visitors[specificObject].Draw(canvas, worldBitmap);
            }

            // Draw elevator 
            if(elevator != null)
            elevator.Draw(canvas, worldBitmap);

            // Draw cleaners
            for (int specificObject = 0; specificObject < lobby.Cleaners.Count; specificObject++)
            {
                lobby.Cleaners[specificObject].Draw(canvas, worldBitmap);
            }
        }

        /// <summary>
        /// Creating factories so we can use them to create objects
        /// Those factories will register them self into a list in the class Factory where there is only one from.
        /// </summary>
        public void CreateFactory()
        {
            ConcreteCreatableObjectFactory concreteAreaTypeFactory = new ConcreteCreatableObjectFactory();
            ConcreteMovableCreaturesFactory concreteHumanFactory = new ConcreteMovableCreaturesFactory();
        }

        /// <summary>
        /// In this class we want to create cleaners because we look at this game like Hotel is the world.
        /// If this game would be bigger, we could create humans in the mainscreen and give them a behaviour for example with the strategy pattern
        /// </summary>
        public void CreateCleaners()
        {
            LocationType location = null;
            foreach (var item in Facilities)
            {
                if(item.Position.X == 1 && item.Position.Y == 0)
                {
                    location = item;
                }
            }
            // List we will send to lobby
            List<Cleaner> cleaners = new List<Cleaner>();
            // Create the factory to create cleaners
            IFactory locationTypeFactory = Factory.AbstractFactory.Instance().Create("MovableCreatures");
            MovableParam initializeParam = new MovableParam() { PathFinding = pathfinding, StartLocation = location, Settings = this.hte };

            // Create two cleaners based on the string
            Cleaner cleaner1 = locationTypeFactory.CreateMovableCreatures("Cleaner", initializeParam) as Cleaner;
            Cleaner cleaner2 = locationTypeFactory.CreateMovableCreatures("Cleaner", initializeParam) as Cleaner;

            // Adding cleaners to the lsit we are going to send
            cleaners.Add(cleaner1);
            cleaners.Add(cleaner2);

            // sending cleaners to lobby
            lobby.GetCleaners(cleaners);
        }

        /// <summary>
        /// This class also is responsible for creating visitors (normally the world/mainscreen when the game would be bigger)
        /// </summary>
        /// <param name="VisitorInfo"></param>
        /// <returns></returns>
        public Visitor CreateVisitor(Dictionary<string, string> visitorInfo)
        {
            // Create the factory to create visitors
            IFactory locationTypeFactory = Factory.AbstractFactory.Instance().Create("MovableCreatures");
            // Visitor his ID 
            string id = null;
            foreach (KeyValuePair<string, string> item in visitorInfo)
            {
                for (int i = 0; i < item.Key.Length; i++)
                {
                    if (Char.IsDigit(item.Key[i]))
                    {
                        id += char.GetNumericValue(item.Key[i]);
                    }
                }            
            }
            MovableParam initialiseParam = new MovableParam() { PathFinding = pathfinding, ID = Convert.ToInt32(id), Settings = this.hte };

            // Create two cleaners based on the string
            Visitor visitor = locationTypeFactory.CreateMovableCreatures("Visitor", initialiseParam) as Visitor;
            lobby.GetVisitor(visitor);

            // Give visitor to lobby
            return visitor;
        }

        /// <summary>
        /// This is where we create objects by reading a file
        /// Bases on the AreaType we can dynamically create objects of this Areatype
        /// We also create objects manually by creating the factory, we can send a string which identifies what kind of object we want to create
        /// We send the facility model to the being created object which he can initialize
        /// </summary>
        /// <param name="file"></param>
        private void CreateFactoryObjects(string file)
        {
            IFactory locationTypeFactory = Factory.AbstractFactory.Instance().Create("LocationType");
            // This fileReader object is made to read files and make objects out of it
            FileReader fileReader = new FileReader();
            // This code lets us write the layout into model
            List<Facility> facilitiesModels = fileReader.ReadLayoutFile(file);

            // We jump right into the models annd for every mode lwe create an object
            foreach (var item in facilitiesModels)
            {
                // Setting hte of the hte value in the facility model
                item.Hte = this.hte;
                // Bases on the areaytype we create items
                LocationType AreaType = locationTypeFactory.CreateCreatableObject(item.AreaType, item) as LocationType;
                // We add the item in an list so we can make use of it
                Facilities.Add(AreaType);
            }



            // get the object with the biggest x-position. Now we can add the x position and Dimension * width to give stairs te correct position
            LocationType biggest = Facilities.Aggregate((i1, i2) => i1.Position.X > i2.Position.X ? i1 : i2);

            // We need the max y-position from the facility list so we can create the correct amount of stairs and elevatorshafts
            int maxYposHotel = Facilities.Max(element => element.Position.Y);

            // We need to know the minimum x-position because if the lowest x-position is zero, then the elevatorshafts has to have a position aswell.
            int minXposHotel = Facilities.Min(element => element.Position.X);

            // If the lowest x-position is zero or lower, we subtract 1, now we got minus 1 or even lesser, depends on how low the x-position is.
            if (minXposHotel <= 0)
            {
                minXposHotel -= 1;
            }
            // Else we keep it at 0, we now can draw multiple layout files without having to worry about the positions given to us.
            else
            {
                minXposHotel = 0;
            }
            // In the method ChangePositionHotel, I will continiue this comment.....




            // Where we will store information to send to locationtype
            Facility specs;

            // Where we will store the created ID. Locationtype will set this as it's ID.
            int CreatedID = 0;

            // This is where lobby is created
            CreatedID = Facilities.Max(element => element.ID) + 1;
            specs = new Facility() { AreaType = "Lobby", Dimension = new Point(biggest.Position.X + Math.Abs(minXposHotel) + biggest.Dimension.X - 1, 1), Position = new Point(1 + minXposHotel, 0), ID = CreatedID, Hte = this.hte };
            LocationType Lobby = locationTypeFactory.CreateCreatableObject("Lobby", specs) as LocationType;
            lobby = Lobby as Lobby;
            Facilities.Add(Lobby);

            // this is the elevatorhall where elevator moves in
            for (int i = 0; i <= maxYposHotel; i++)
            {
                CreatedID = Facilities.Max(element => element.ID) + 1;
                specs = new Facility() { AreaType = "ElevatorHall", Position = new Point(minXposHotel, i), Dimension = new Point(1, 1), ID = CreatedID, Hte = this.hte };
                LocationType elevatorHall = locationTypeFactory.CreateCreatableObject("ElevatorHall", specs) as LocationType;
                Facilities.Add(elevatorHall);

                if (i == 0)
                {
                    specs = new Facility() { ElevatorShaft = elevatorHall, Hte = this.hte };
                    elevator = locationTypeFactory.CreateCreatableObject("Elevator", specs) as Elevator;
                }
                (elevatorHall as ElevatorShaft).Attach(elevator);
            }

            // Staircase will be set on the last row of the hotel
            for (int i = 0; i <= maxYposHotel; i++)
            {
                CreatedID = Facilities.Max(element => element.ID) + 1;
                specs = new Facility() { AreaType = "Staircase", Position = new Point((biggest.Position.X + biggest.Dimension.X), i), Dimension = new Point(1, 1), ID = CreatedID, Hte = this.hte };
                LocationType stairCase = locationTypeFactory.CreateCreatableObject("Staircase", specs) as LocationType;
                Facilities.Add(stairCase);
            }


            // Give Lobby the rooms because we see it as the reception, so the reception has to manage visitors. 
            // Visitors dont leave the place so their world is all facilities combined.
            for (int i = 0; i < Facilities.Count; i++)
            {
                if (Facilities[i].AreaType.Equals("Room"))
                {
                    lobby.GetRoom((Facilities[i] as Room));
                }
            }
            pathfinding = new PathFinding(Facilities);
            // Change position from hotel if there are negative coordinations (like position.x = -1)
            ChangePositionLocationtypes();
            InitialiseHotelDimension();
        }

        /// <summary>
        /// We move the positions based on the lowest positions we can get
        /// So let's say that the lowest x position is -5, we say -5 will be 5
        /// We will loop trought all facilities and sum 5 so now the lowest x position will be 0
        /// We draw based on the oneDimensionwidth by multiplying it with the x position
        /// so oneDimensionwdith(150) x 0 = zero and that will be the most left item we will see
        /// </summary>
        public void ChangePositionLocationtypes()
        {
            // Get the minimum x-position from the facility list
            int minXposHotel = Facilities.Min(element => element.Position.X);

            // if the minumum x-position is zero or smaller
            if (minXposHotel < 0)
            {
                // We make from the number, for example -1, 1;
                int positiveNumber = Math.Abs(minXposHotel);
                // We now add at every x-position from a facility, 1. Now the facilities with -1, will have a x-position of 0. And that is excactly what we need to draw the hotel correct.
                for (int specificObject = 0; specificObject < Facilities.Count; specificObject++)
                {
                    Facilities[specificObject].SetPosition(new Point(Facilities[specificObject].Position.X + positiveNumber, Facilities[specificObject].Position.Y));
                }
            }
        }

        /// <summary>
        /// Based on the highest xpos en ypos off a locationtype we will set the height and with off the hotel
        /// We do this everytime the hotel is being changed for example being attacked by the godzilla
        /// </summary>
        public void InitialiseHotelDimension()
        {
            // take the bigges coordinations from the facilities
            if(Facilities.Count != 0)
            { 
                LocationType BiggestYPos = Facilities.Aggregate((i1, i2) => i1.Position.Y > i2.Position.Y ? i1 : i2);
                LocationType BiggestXPos = Facilities.Aggregate((i1, i2) => i1.Position.X > i2.Position.X ? i1 : i2);

                // set the height and width from the hotel dynamically
                HotelHeight = (BiggestYPos.Position.Y * BiggestYPos.oneDimensionSize.Height) + ((BiggestYPos.Dimension.Y + 1) * BiggestYPos.oneDimensionSize.Height);
                HotelWidth = (BiggestXPos.Position.X * BiggestXPos.oneDimensionSize.Width) + ((BiggestXPos.Dimension.X) * BiggestXPos.oneDimensionSize.Width);
            }
        }

        /// <summary>
        /// We connect areatypes based on their positions for the purpose off the pathfinding
        /// </summary>
        private void LinkLocationTypes()
        {
            // Variables we need to link locationtypes to eachother
            LocationType current = null;
            LocationType next = null;
            // We use those numbers to make the programm more dynamically
            int maxYposHotel = Facilities.Max(element => element.Position.Y); // get the max height from hotel that is : the position.Y = 6
            int maxXposHotel = Facilities.Max(element => element.Position.X); // get the max with from hotel that is : the position.X = 9
            int minXposHotel = Facilities.Min(element => element.Position.X); // get the minimum width from hotel that is : the position.X = 0
            int minYposHotel = Facilities.Min(element => element.Position.Y); // get the minimum height from hotel that is : the position.Y = 0
            int biggestXDimension = Facilities.Max(element => element.Dimension.X); // get the location type with the biggest Dimension.X = 8;

            // Count from the smallest Y position till the biggest Y position (from floor 0, till highest floor)
            for (int heightHotel = minYposHotel; heightHotel <= maxYposHotel; heightHotel++) 
            {
                // Count from the smallest X position till the highest X position
                for (int widthHotel = (minXposHotel); widthHotel <= maxXposHotel; widthHotel++) 
                {
                    // When current is zero, and it becomes zero when we looped a full row
                    if (current == null) 
                    {
                        // Current becomes the first possible item in a row, it will keep searching with the forloop, so eventually i'll find it's position
                        current = SearchLocationType(widthHotel, heightHotel); 
                    }
                    // When current is not zero
                    else
                    {
                        // current Dimension is one, because we want to search the next item and the next item might not be 1 position ahead but maybe 8, because Dimensions will take some space
                        for (int currentDimensionCount = 1; currentDimensionCount <= biggestXDimension; currentDimensionCount++) 
                        {
                            // Search for next item by adding to it's position 1 untill the the number is as big as the biggest Dimension and in the meanwhile there is a chance we will find the next item
                            next = SearchLocationType((current.Position.X + currentDimensionCount), current.Position.Y); // next becomes the next item. Example lobby : 1 + 8 = 9 (next is stairs)
                            if (next != null)
                            {
                                // When we found the next item, let's get out of this loop, for performance reasons
                                break;
                            }
                        }
                        // When we have found the next item
                        if (next != null) 
                        {
                            // Check if it's an elevatorshaft, because 
                            if (current is ElevatorShaft) // if position from current is 9
                            {
                                // Get the upper Neighbor
                                LocationType aboveNeighbor = SearchLocationType(current.Position.X, current.Position.Y + 1);
                                // Get the neighbor below
                                LocationType underNeighbor = SearchLocationType(current.Position.X, current.Position.Y - 1);

                                // Add the neighbors
                                if(aboveNeighbor != null)
                                   current.SetNeighbor(aboveNeighbor, 1);
                                if(underNeighbor != null)
                                   current.SetNeighbor(underNeighbor, 1);
                            }

                            // We place this here because elevatorshaft can have a next neighbor, while staircase can't have that.
                            current.Neighbor.Add(next, next.Position.X - current.Position.X); // add the next item to the current one
                            next.Neighbor.Add(current, next.Position.X - current.Position.X); // add the current item to the next one
                            current = next; // current becomes the next one, we now count from there

                            if (current is Staircase) // if position from current is 9
                            {
                                // Get the upper Neighbor
                                LocationType aboveNeighbor = SearchLocationType(current.Position.X, current.Position.Y + 1);
                                // Get the neighbor below
                                LocationType underNeighbor = SearchLocationType(current.Position.X, current.Position.Y - 1);

                                // Add the neighbors
                                if (aboveNeighbor != null)
                                    current.SetNeighbor(aboveNeighbor, 3);
                                if (underNeighbor != null)
                                    current.SetNeighbor(underNeighbor, 3);
                            }
                        }
                        else
                        {
                            widthHotel = maxXposHotel;
                        }
                    }
                }
                current = null; // current should become null, because we want to give it a new position, one row higher
            }
            // For testing purposes
            foreach (var item in Facilities)
            {
                //Console.WriteLine();
                //Console.WriteLine(item.AreaType + " : " + item.Position);

                //foreach (var itemm in item.NeighBor)
                //{
                //    Console.WriteLine(itemm.Key.AreaType + " : " + itemm.Key.Position);
                //}
                //Console.WriteLine();
            }
        }

        /// <summary>
        /// Because we use this "function" a lot we made a method out of it
        /// You can search a locationtype based on the xposition and yposition
        /// </summary>
        /// <param name="xPos"></param>
        /// <param name="yPos"></param>
        /// <returns></returns>
        private LocationType SearchLocationType(int xPos, int yPos)
        {
            // Use the xPos and the yPos to find an item
            Point point = new Point(xPos, yPos);

            foreach (LocationType item in Facilities) // Search the cacilities list
            {
                if (item.Position == point) // When the position in the list is the same as the position we gave it to here
                {
                    return item; // we now we can use the item to set variabels on
                }
            }
            // else return null
            return null;
        }
    }
}

