using HotelSimulatie.Actors;
using HotelSimulatie.Facilities;
using HotelSimulatie.Factory;
using HotelSimulatie.Graph;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelSimulatie
{
    class Hotel
    {
        public List<Node> Facilities { get; set; }
        private List<Node> rooms;

        public Hotel()
        {
            Facilities = new List<Node>();
            rooms = new List<Node>();
        }

        /// <summary>
        /// Hier worden hardcoded/dynamisch plekken toegevoegd aan de lijst met plekken
        /// </summary>
        public void AssembleHotel()
        {
            #region Handmatig toevoegen hier:

            // Geef lobby een lijst met kamers
            Lobby lobby = new Lobby(rooms);
            lobby.Position = new Point(0, 0);

            LobbyExtension lobbyExtension = new LobbyExtension();
            lobbyExtension.Position = new Point(1, 0);

            Elevator elevator0 = new Elevator();
            elevator0.Position = new Point(0, 7);

            Elevator elevator1 = new Elevator();
            elevator1.Position = new Point(0, 6);

            Elevator elevator2 = new Elevator();
            elevator2.Position = new Point(0, 5);

            Elevator elevator3 = new Elevator();
            elevator3.Position = new Point(0, 4);

            Elevator elevator4 = new Elevator();
            elevator4.Position = new Point(0, 3);

            Elevator elevator5 = new Elevator();
            elevator5.Position = new Point(0, 2);

            Elevator elevator6 = new Elevator();
            elevator6.Position = new Point(0, 1);

            int etageStair = 1;
            for (int i = 0; i < 6; i++)
            {
                Staircase stairCase = new Staircase();
                stairCase.Position = new Point(9, etageStair);
                stairCase.Dimension = new Point(1, 1);
                Facilities.Add(stairCase);
                etageStair++;
                // Voeg de plekken die je handmatig hebt aangemaakt toe aan de lijst
            }

            Facilities.Add(lobby);
            Facilities.Add(lobbyExtension);
            Facilities.Add(elevator0);
            Facilities.Add(elevator1);
            Facilities.Add(elevator2);
            Facilities.Add(elevator3);
            Facilities.Add(elevator4);
            Facilities.Add(elevator5);
            Facilities.Add(elevator6);

            #region Factory method
            Creator[] Creator = new Creator[1];

            Creator[0] = new ConcreteAbElevatorCreator();
            //Creator[1] = new ConcreteCreatorB();

            foreach (Creator creator in Creator)
            {
                //MyElevator concreteElevator = new MyElevator();
                MyElevator concreteElevator = creator.FactoryMethod();
                Console.WriteLine("Created {0}", concreteElevator.GetType().Name);
            }
            #endregion

            #endregion

            #region vergroot sommige plekken 

            List<Node> EmptySpots = new List<Node>();
            foreach (var item in Facilities)
            {
                // Als er een plek is die groter is dan 1x1
                if ((item.Dimension.X > 1) || (item.Dimension.Y > 1))
                {
                    Point tempPoint;
                    Extension visualHallway = new Extension();
                    int newX = 0;
                    int newY = 0;

                    // 2, 2

                    for (int i = 0; i < (item.Dimension.X - 1); i++)
                    {
                        newX = item.Position.X + 1;
                        newY = item.Position.Y;
                        tempPoint = new Point(newX, newY);
                        visualHallway.Position = tempPoint;
                        if (item is Restaurant)
                        {
                            visualHallway.TileImage = Image.FromFile("../../Resources/restaurant_extension.png");
                        }
                        else
                        {
                            visualHallway.TileImage = Image.FromFile("../../Resources/room_extension.png");
                        }
                        EmptySpots.Add(visualHallway);
                    }

                }

            }

            foreach (var item in EmptySpots)
            {
                Facilities.Add(item);
            }
            
            #endregion
        }
        
        /// <summary>
        /// Ontvangt een lijst met data modellen uit FileReader.
        /// Sorteert de modellen in echte classes/plekken.
        /// </summary>
        public void SortList()
        {
            #region Create objects from the list of model facilities

            FileReader fileReader = new FileReader();

            List<Facility> facilitiesModels = fileReader.ReadLayoutFile();

            // Haal alle faciliteiten uit de model list van faciliteiten 
            foreach (var item in facilitiesModels)
            {
                if (item.AreaType.Equals("Cinema"))
                {
                    Cinema cinema = new Cinema();
                    cinema.Position = item.Position;
                    cinema.Dimension = item.Dimension;
                    Facilities.Add(cinema);
                }
                else if (item.AreaType.Equals("Restaurant"))
                {
                    Restaurant restaurant = new Restaurant();
                    restaurant.Capacity = item.Capacity;
                    restaurant.Position = item.Position;
                    restaurant.Dimension = item.Dimension;
                    Facilities.Add(restaurant);
                }
                else if (item.AreaType.Equals("Room"))
                {
                    Room room = new Room();
                    room.Position = item.Position;
                    room.Dimension = item.Dimension;
                    room.Classification = item.Classification;
                    rooms.Add(room);
                    Facilities.Add(room);
                }
                else
                {
                    Fitnesscentrum fitnessCentrum = new Fitnesscentrum();
                    fitnessCentrum.Position = item.Position;
                    fitnessCentrum.Dimension = item.Dimension;
                    Facilities.Add(fitnessCentrum);
                }
            }

            #endregion
        }
    }
}
