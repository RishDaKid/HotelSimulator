using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using HotelSimulatie;
using System.Collections.Generic;
using System.Drawing;
using HotelSimulatie.Enums;
using HotelSimulatie.Facilities;
using System.Diagnostics;
using HotelSimulatie.Factory;
using HotelSimulatie.Model;

namespace HotelTests
{
    [TestClass]
    public class ElevatorTests
    {
        private Elevator myElevator;
        private PrivateObject elevator;
        private bool result;
        private List<ElevatorShaft> shafts;
        Facility specs;
        ConcreteCreatableObjectFactory concreteAreaTypeFactory = new ConcreteCreatableObjectFactory();

        [TestInitialize]
        public void Initialize()
        {
            shafts = new List<ElevatorShaft>();
            GenerateElevatorRooms();
           
            elevator = new PrivateObject(myElevator);
            elevator.SetField("location", shafts[3]);
        }

        // Environment to test the elevator in.
        public void GenerateElevatorRooms()
        {
            FileReader hteReader = new FileReader();
            // Read the settingsfile into HTE value
            Settings hte = hteReader.GetSettings();

            IFactory locationTypeFactory = AbstractFactory.Instance().Create("LocationType");

            for (int i = 0; i < 10; i++)
            {
                specs = new Facility() { AreaType = "ElevatorHall", Position = new Point(0, i), Dimension = new Point(1, 1) };
                ElevatorShaft elevatorHall = locationTypeFactory.CreateCreatableObject("ElevatorHall", specs) as ElevatorShaft;
                shafts.Add(elevatorHall);

                if (i == 0)
                {
                    specs = new Facility() { ElevatorShaft = elevatorHall, Hte = hte };
                    myElevator = locationTypeFactory.CreateCreatableObject("Elevator", specs) as Elevator;
                }
            }
            for (int i = 0; i < 10; i++)
                if (i != 9)
                    shafts[i].Neighbor.Add(shafts[i + 1], 1);
        }

        // Visually represent two lists
        public void Print(List<Request> one, List<Request> two)
        {
            Debug.WriteLine("\nIn elevator.Requests:");
            foreach (Request request in one)
            {
                Debug.WriteLine(request.Floor + ", " + request.ButtonState);
            }

            Debug.WriteLine("\nIn ExpectedOrder:");
            foreach (Request request in two)
            {
                Debug.WriteLine(request.Floor + ", " + request.ButtonState);
            }
        }

        /// <summary>
        /// Test purpose: Check if the elevator can traverse elevator shafts and arrive at different elevator room.
        /// </summary>
        [TestMethod]
        public void Elevator_Can_Elevator_Move()
        {
            // arrange
            elevator.SetField("location", shafts[0]);
            elevator.SetField("floor", 0);

            Request request = new Request(1, Direction.UP);
            int targetFloor = (int)elevator.GetField("floor") + 1;

            // act

            elevator.Invoke("HandleCall", request);
            elevator.Invoke("Activate");

            // assert
            Assert.AreEqual((int)elevator.GetField("floor"), targetFloor); // Check floor int
            Assert.AreEqual((ElevatorShaft)elevator.GetField("location"), shafts[targetFloor]); // Check for LocationType change
        }

        /// <summary>
        /// Test purpose: Confirms that the HandleCall method in Elevator prevents duplicates. 
        /// </summary>
        [TestMethod]
        public void Elevator_HandleCall_contains_one_request_when_given_a_Duplicate()
        {
            // arrange
            Request firstRequest = new Request(1, Direction.UP);
            Request duplicate = new Request(1, Direction.UP);

            // act
            elevator.Invoke("HandleCall", firstRequest);
            elevator.Invoke("HandleCall", duplicate);
            List<Request> list = (List<Request>)elevator.GetField("requests");

            // assert
            Assert.AreEqual((list.Count == 1), true); // True if duplicate is found
        }

        /// <summary>
        /// Test purpose: Tests confirms that the elevator can perform a sweeping motion in an attempt to try to service all requests in 
        /// one direction before heading in the opposite way.
        /// </summary>
        [TestMethod]
        public void Elevator_Sort_Simple_Direction_Test()
        {
            // arrange

            elevator.SetField("location", shafts[3]);
            elevator.SetField("floor", 3);

            List<Request> ExpectedOrder = new List<Request>()
            {
                new Request(8, Direction.UP),
                new Request(12, Direction.UP),
                new Request(11, Direction.DOWN),
                new Request(10, Direction.DOWN),
                new Request(8, Direction.DOWN),
                new Request(6, Direction.DOWN),
                new Request(1, Direction.UP)
            };

            elevator.Invoke("HandleCall", ExpectedOrder[3]);
            elevator.Invoke("HandleCall", ExpectedOrder[4]);
            elevator.Invoke("HandleCall", ExpectedOrder[5]);
            elevator.Invoke("HandleCall", ExpectedOrder[0]);
            elevator.Invoke("HandleCall", ExpectedOrder[1]);
            elevator.Invoke("HandleCall", ExpectedOrder[6]);
            elevator.Invoke("HandleCall", ExpectedOrder[2]);

            // act

            // assert
            CollectionAssert.AreEqual((List<Request>)elevator.GetField("requests"), ExpectedOrder);
        }

        /// <summary>
        /// Test purpose: Confirms that the sorting algorithm can handle requests from both direction regardless of the buttonstate
        /// of that request.
        /// </summary>
        [TestMethod]
        public void Elevator_Sort_Advanced_Test()
        {
            // arrange

            elevator.SetField("location", shafts[3]);
            elevator.SetField("floor", 3);

            List<Request> ExpectedOrder = new List<Request>()
            {
                new Request(8, Direction.UP),
                new Request(9, Direction.NONE),
                new Request(12, Direction.UP),
                new Request(60, Direction.DOWN),
                new Request(50, Direction.DOWN),
                new Request(30, Direction.DOWN),
                new Request(20, Direction.DOWN),
                new Request(15, Direction.DOWN),
                new Request(11, Direction.DOWN),
                new Request(10, Direction.DOWN),
                new Request(8, Direction.DOWN),
                new Request(6, Direction.DOWN),
                new Request(1, Direction.UP)
            };

            elevator.Invoke("HandleCall", ExpectedOrder[3]);
            elevator.Invoke("HandleCall", ExpectedOrder[4]);
            elevator.Invoke("HandleCall", ExpectedOrder[5]);
            elevator.Invoke("HandleCall", ExpectedOrder[0]);

            elevator.Invoke("HandleCall", ExpectedOrder[1]);
            elevator.Invoke("HandleCall", ExpectedOrder[6]);
            elevator.Invoke("HandleCall", ExpectedOrder[2]);
            elevator.Invoke("HandleCall", ExpectedOrder[7]);
            elevator.Invoke("HandleCall", ExpectedOrder[8]);
            elevator.Invoke("HandleCall", ExpectedOrder[9]);
            elevator.Invoke("HandleCall", ExpectedOrder[12]);
            elevator.Invoke("HandleCall", ExpectedOrder[10]);
            elevator.Invoke("HandleCall", ExpectedOrder[11]);

            // act
            // ...

            // assert
            CollectionAssert.AreEqual((List<Request>)elevator.GetField("requests"), ExpectedOrder);
        }

        /// <summary>
        /// A simple test to see if SCAN works in both direction.
        /// </summary>
        [TestMethod]
        public void Elevator_Sort_Scan_Look_Reversed_Test() // Set floor to 8 first
        {
            // arrange

            elevator.SetField("location", shafts[9]);
            elevator.SetField("floor", 9);

            List<Request> ExpectedOrder = new List<Request>()
            {
                new Request(7, Direction.DOWN), // [0]
                new Request(6, Direction.NONE), // Does not get added!
                new Request(4, Direction.DOWN),
                new Request(3, Direction.DOWN),
                new Request(2, Direction.DOWN), // [5]
                new Request(0, Direction.DOWN),
                new Request(5, Direction.UP),
                new Request(25, Direction.UP),  // [8]
                new Request(22, Direction.DOWN),
                new Request(20, Direction.DOWN),
                new Request(18, Direction.DOWN), // [10]
                new Request(16, Direction.DOWN),
                new Request(15, Direction.DOWN)
            };

            elevator.Invoke("HandleCall", ExpectedOrder[3]);
            elevator.Invoke("HandleCall", ExpectedOrder[4]);
            elevator.Invoke("HandleCall", ExpectedOrder[5]);
            elevator.Invoke("HandleCall", ExpectedOrder[0]);

            elevator.Invoke("HandleCall", ExpectedOrder[1]);
            elevator.Invoke("HandleCall", ExpectedOrder[6]);
            elevator.Invoke("HandleCall", ExpectedOrder[2]);
            elevator.Invoke("HandleCall", ExpectedOrder[7]);
            elevator.Invoke("HandleCall", ExpectedOrder[8]);
            elevator.Invoke("HandleCall", ExpectedOrder[9]);
            elevator.Invoke("HandleCall", ExpectedOrder[12]);
            elevator.Invoke("HandleCall", ExpectedOrder[10]);
            elevator.Invoke("HandleCall", ExpectedOrder[11]);

            // act
            // ...

            // assert
            //Print(elevator.Requests, ExpectedOrder);
            CollectionAssert.AreEqual((List<Request>)elevator.GetField("requests"), ExpectedOrder);
        }

        [TestMethod]
        public void Elevator_Sort_Simple_Test()
        {
            // arrange

            elevator.SetField("location", shafts[5]);
            elevator.SetField("floor", 5);

            List<Request> ExpectedOrder = new List<Request>()
            {
                new Request(3, Direction.DOWN),
                new Request(6, Direction.UP)
            };

            elevator.Invoke("HandleCall", ExpectedOrder[0]);
            elevator.Invoke("HandleCall", ExpectedOrder[1]);

            // act

            // assert
            CollectionAssert.AreEqual((List<Request>)elevator.GetField("requests"), ExpectedOrder);
        }


        [TestMethod]
        public void Elevator_Sort_Scan_Advanced_Test()
        {
            // arrange
            elevator.SetField("location", shafts[3]);
            elevator.SetField("floor", 3);

            List<Request> ExpectedOrder = new List<Request>()
            {
                new Request(8, Direction.UP),
                new Request(12, Direction.UP),
                new Request(11, Direction.DOWN),
                new Request(10, Direction.DOWN),
                new Request(8, Direction.DOWN),
                new Request(6, Direction.DOWN),
                new Request(1, Direction.UP)
            };

            elevator.Invoke("HandleCall", ExpectedOrder[3]);
            elevator.Invoke("HandleCall", ExpectedOrder[4]);
            elevator.Invoke("HandleCall", ExpectedOrder[5]);
            elevator.Invoke("HandleCall", ExpectedOrder[0]);
            elevator.Invoke("HandleCall", ExpectedOrder[1]);
            elevator.Invoke("HandleCall", ExpectedOrder[6]);
            elevator.Invoke("HandleCall", ExpectedOrder[2]);

            // act
            // ...

            // assert
            CollectionAssert.AreEqual((List<Request>)elevator.GetField("requests"), ExpectedOrder);
        }

        [TestMethod]
        public void Elevator_Sort_Scan_Wiki_Test()
        {
            // arrange
            elevator.SetField("floor", 53);

            List<Request> ExpectedOrder = new List<Request>()
            {
                new Request(65, Direction.NONE),
                new Request(67, Direction.NONE),
                new Request(98, Direction.NONE),
                new Request(122, Direction.NONE),
                new Request(124, Direction.NONE),
                new Request(183, Direction.NONE),
                new Request(37, Direction.NONE),
                new Request(14, Direction.NONE),
            };

            elevator.Invoke("HandleCall", ExpectedOrder[3]);
            elevator.Invoke("HandleCall", ExpectedOrder[4]);
            elevator.Invoke("HandleCall", ExpectedOrder[5]);
            elevator.Invoke("HandleCall", ExpectedOrder[0]);
            elevator.Invoke("HandleCall", ExpectedOrder[1]);
            elevator.Invoke("HandleCall", ExpectedOrder[6]);
            elevator.Invoke("HandleCall", ExpectedOrder[7]);
            elevator.Invoke("HandleCall", ExpectedOrder[2]);

            // act

            // assert
            //Print(elevator.Requests, ExpectedOrder);
            CollectionAssert.AreEqual((List<Request>)elevator.GetField("requests"), ExpectedOrder);
        }

        [TestMethod]
        public void Elevator_Sort_Scan_Reverse_Wiki_Test()
        {
            // arrange
            elevator.SetField("floor", 53);

            List<Request> ExpectedOrder = new List<Request>()
            {
                new Request(37, Direction.NONE),
                new Request(14, Direction.NONE),
                new Request(65, Direction.NONE),
                new Request(67, Direction.NONE),
                new Request(98, Direction.NONE),
                new Request(122, Direction.NONE),
                new Request(124, Direction.NONE),
                new Request(183, Direction.NONE),
            };

            elevator.Invoke("HandleCall", ExpectedOrder[0]);
            elevator.Invoke("HandleCall", ExpectedOrder[4]);
            elevator.Invoke("HandleCall", ExpectedOrder[5]);
            elevator.Invoke("HandleCall", ExpectedOrder[7]);
            elevator.Invoke("HandleCall", ExpectedOrder[1]);
            elevator.Invoke("HandleCall", ExpectedOrder[6]);
            elevator.Invoke("HandleCall", ExpectedOrder[3]);
            elevator.Invoke("HandleCall", ExpectedOrder[2]);
            // act

            // assert
            //Print(elevator.Requests, ExpectedOrder);
            CollectionAssert.AreEqual((List<Request>)elevator.GetField("requests"), ExpectedOrder);
        }
    }
}
