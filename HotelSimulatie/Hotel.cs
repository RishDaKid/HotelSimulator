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

namespace HotelSimulatie
{
    public class Hotel
    {
        public int HotelHeight { get; set; }
        public int HotelWidth { get; set; }
        public Point HotelPosition { get; set; }
        public List<LocationType> Facilities { get; set; }
        public List<LocationType> Rooms { get; set; }
        public PathFinding pathFinding { get; set; }

        public Hotel()
        {
            HotelPosition = new Point(160, 182);
            HotelHeight = 834;
            HotelWidth = 1600;
            Facilities = new List<LocationType>();
            Rooms = new List<LocationType>();
            CreateFactories();
            LinkLocationTypes();
            pathFinding = new PathFinding(Facilities);
        }

        private void CreateFactories()
        {
            CinemaFactory cinemaFactory = new CinemaFactory();
            FitnessFactory fitnessFactory = new FitnessFactory();
            RestaurantFactory restaurantFactory = new RestaurantFactory();
            RoomFactory roomFactory = new RoomFactory();

            MovableFactory movableFactory = new MovableFactory();
            movableFactory.RegisterFactory("Cinema", cinemaFactory);
            movableFactory.RegisterFactory("Fitness", fitnessFactory);
            movableFactory.RegisterFactory("Restaurant", restaurantFactory);
            movableFactory.RegisterFactory("Room", roomFactory);

            CreateFactoryObjects(movableFactory);
        }

        private void CreateFactoryObjects(MovableFactory movableFactory)
        {
            FileReader fileReader = new FileReader();
            // Create Objects with facility model
            List<Facility> facilitiesModels = fileReader.ReadLayoutFile();

            foreach (var item in facilitiesModels)
            {
                LocationType locationType = movableFactory.Create(item);
                locationType.AreaType = item.AreaType;
                locationType.Dimension = item.Dimension;
                locationType.Position = item.Position;
                Facilities.Add(locationType);
            }
            // Create Objects which are not facility models
            Lobby lobby = new Lobby();
            lobby.AreaType = "Lobby";
            lobby.Position = new Point(1, 0);
            lobby.Dimension = new Point(8, 1);
            Facilities.Add(lobby);

            int etageStair = 0;
            for (int i = 0; i < 7; i++)
            {
                Staircase stairCase = new Staircase();
                stairCase.AreaType = "Staircase";
                stairCase.Position = new Point(9, etageStair);
                stairCase.Dimension = new Point(1, 1);
                Facilities.Add(stairCase);
                etageStair++;
            }

            int etageElevator = 0;
            for (int i = 0; i < 7; i++)
            {
                ElevatorHall elevatorHall = new ElevatorHall();
                elevatorHall.AreaType = "ElevatorHall";
                elevatorHall.Position = new Point(0, etageElevator);
                elevatorHall.Dimension = new Point(1, 1);
                Facilities.Add(elevatorHall);
                etageElevator++;
            }

            Elevator elevator = new Elevator();
            elevator.AreaType = "Elevator";
            elevator.Position = new Point(0, 0);
            elevator.Dimension = new Point(1, 1);
            Facilities.Add(elevator);
            etageElevator++;

            foreach (var item in Rooms)
            {
                Facilities.Add(item);
            }
        }
        // 
        private void LinkLocationTypes()
        {
            LocationType current = new LocationType();
            LocationType next = new LocationType();

            int maxHeightHotel = Facilities.Max(element => Math.Abs(element.Position.Y)); // get the max height from hotel that is : the position.Y = 6
            int maxWidthHotel = Facilities.Max(element => Math.Abs(element.Position.X)); // get the max with from hotel that is : the position.X = 9
            int minWidthHotel = Facilities.Min(element => Math.Abs(element.Position.X)); // get the minimum width from hotel that is : the position.X = 0
            int minHeightHotel = Facilities.Min(element => Math.Abs(element.Position.Y)); // get the minimum height from hotel that is : the position.Y = 0
            int maxDimentionLocationType = Facilities.Max(element => Math.Abs(element.Dimension.X)); // get the location type with the biggest dimention.X = 8;


            for (int heightHotel = minHeightHotel; heightHotel <= maxHeightHotel; heightHotel++) // Count from (min floor) till max height (max floor)
            {
                for (int widthHotel = (minWidthHotel + 1); widthHotel <= maxWidthHotel; widthHotel++) // Count afther elevator collumn so, 1, till max (max collums) == 9
                {
                    if (current == null) // Current starts with zero
                    {
                        current = SearchLocationType(widthHotel, heightHotel); // Current becomes the first possible item in a row, it will keep searching with the forloop, so eventually i'll find it's position
                    }
                    else
                    {
                        for (int currentDimentionCount = (minWidthHotel + 1); currentDimentionCount <= maxDimentionLocationType; currentDimentionCount++) // Count from 1 till the widest locationType dimention == 8, to get the next item
                        {
                            next = SearchLocationType((current.Position.X + currentDimentionCount), current.Position.Y); // next becomes the next item. Example lobby : 1 + 8 = 9 (next is stairs)
                            if(next != null)
                            {
                                break;
                            }
                        }
                        if (next != null) // When we found the next item, counting from current item
                        {
                            current.neighBor.Add(next, next.Position.X - current.Position.X); // add too the current item, the next item
                            next.neighBor.Add(current, next.Position.X - current.Position.X); // add too the next item, the current item
                            current = next; // current becomes the next one, we now count from there

                            if (current.Position.X == maxWidthHotel) // if position from current is 9
                            {
                                next = SearchLocationType(current.Position.X, current.Position.Y + 1);
                                if (next != null)
                                {
                                    current.neighBor.Add(next, 1);
                                }
                                next = SearchLocationType(current.Position.X, current.Position.Y - 1);
                                if (next != null)
                                {
                                    current.neighBor.Add(next, 1);
                                }
                            }
                            next = null; // next becomes null, we 
                        }
                    }
                }
                current = null; // current should become null, because we want to give it a new position, one row higher
            }
        }

        private LocationType SearchLocationType(int xPos, int yPos)
        {
            Point point = new Point(xPos, yPos);

            foreach (LocationType item in Facilities) // Search the cacilities list
            {
                if (item.Position == point) // When the position in the list is the same as the position we gave it to here
                {
                    return item; // we now we can use the item to set variabels on
                }
            }
            return null;
        }
    }
}
// http://www.gdunlimited.net/forums/gallery/image/1535-sc-door-jp01-png/