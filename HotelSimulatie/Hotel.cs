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

namespace HotelSimulatie
{
    class Hotel
    {
        private List<LocationType> facilities;
        private List<LocationType> rooms;
        private Point hotelPosition;
        private int heightHotel;
        private int widthHotel;

        public int _heightHotel { get { return heightHotel; } set { } }
        public int _widthHotel { get { return widthHotel; } set { } }
        public Point _hotelPosition { get { return hotelPosition; } set { } }
        public List<LocationType> _facilities
        {
            get { return facilities; }
            set { }
        }
        public List<LocationType> _rooms
        {
            get { return rooms; }
            set { }
        }

        public Hotel()
        {
            hotelPosition = new Point(160, 182);
            heightHotel = 834;
            widthHotel = 1600;
            facilities = new List<LocationType>();
            rooms = new List<LocationType>();
            CreateFactorys();
        }

        public void CreateFactorys()
        {
            ConcreteLocationTypeFactory concreteLocationTypeFactory = new ConcreteLocationTypeFactory();
            MovableFactory movableFactory = new MovableFactory();
            movableFactory.RegisterFactory("AreaType", concreteLocationTypeFactory);
            CreateFactoryObjects(movableFactory);
        }

        public void CreateFactoryObjects(MovableFactory movableFactory)
        {
            // Create Objects with facility model
            FileReader fileReader = new FileReader();
            List<Facility> facilitiesModels = fileReader.ReadLayoutFile();

            foreach (var item in facilitiesModels)
            {
                if (item.AreaType.Equals("Cinema"))
                {
                    Cinema locationType = movableFactory.Create("AreaType",item.AreaType) as Cinema;
                    locationType.AreaType = item.AreaType;
                    locationType.Position = item.Position;
                    locationType.Dimension = item.Dimension;
                    facilities.Add(locationType);
                }
                else if (item.AreaType.Equals("Restaurant"))
                {
                    Restaurant locationType = movableFactory.Create("AreaType", item.AreaType) as Restaurant;
                    locationType.AreaType = item.AreaType;
                    locationType.Capacity = item.Capacity;
                    locationType.Position = item.Position;
                    locationType.Dimension = item.Dimension;
                    facilities.Add(locationType);
                }
                else if (item.AreaType.Equals("Room"))
                {
                    Room locationType = movableFactory.Create("AreaType", item.AreaType) as Room;
                    locationType.AreaType = item.AreaType;
                    locationType.Position = item.Position;
                    locationType.Dimension = item.Dimension;
                    locationType.Classification = item.Classification;
                    facilities.Add(locationType);
                }
                else if (item.AreaType.Equals("Fitness"))
                {
                    Fitnesscentrum locationType = movableFactory.Create("AreaType", item.AreaType) as Fitnesscentrum;
                    locationType.AreaType = item.AreaType;
                    locationType.Position = item.Position;
                    locationType.Dimension = item.Dimension;
                    facilities.Add(locationType);
                }
            }


            // Create Objects which are not facility models
            Lobby lobby = movableFactory.Create("AreaType","Lobby") as Lobby;
            lobby.AreaType = "Lobby";
            lobby.Position = new Point(1, 0);
            lobby.Dimension = new Point(8, 1);
            facilities.Add(lobby);

            int etageStair = 0;
            for (int i = 0; i < 7; i++)
            {
                Staircase stairCase = movableFactory.Create("AreaType", "Staircase") as Staircase;
                stairCase.AreaType = "Staircase";
                stairCase.Position = new Point(9, etageStair);
                stairCase.Dimension = new Point(1, 1);
                facilities.Add(stairCase);
                etageStair++;
            }

            int etageElevator = 0;
            for (int i = 0; i < 7; i++)
            {
                ElevatorHall elevatorHall = movableFactory.Create("AreaType", "ElevatorHall") as ElevatorHall;
                elevatorHall.AreaType = "ElevatorHall";
                elevatorHall.Position = new Point(0, etageElevator);
                elevatorHall.Dimension = new Point(1, 1);
                facilities.Add(elevatorHall);
                etageElevator++;
            }

            Elevator elevator = movableFactory.Create("AreaType", "Elevator") as Elevator;
            elevator.AreaType = "Elevator";
            elevator.Position = new Point(0, 0);
            elevator.Dimension = new Point(1, 1);
            facilities.Add(elevator);
            etageElevator++;

            foreach (var item in rooms)
            {
                facilities.Add(item);
            }
        }

        public void LinkLocationTypes()
        {

        }

    }
}
// http://www.gdunlimited.net/forums/gallery/image/1535-sc-door-jp01-png/