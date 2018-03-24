using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HotelSimulatie.Actors;
using System.Drawing;
using HotelSimulatie.Model;
using HotelSimulatie.Interfaces;
using HotelSimulatie.Enums;
using System.Timers;
using System.Diagnostics; // remove
using System.IO; // remove

namespace HotelSimulatie.Facilities
{
    public class Elevator : CreatableObject, IListener
    {
        private PointF position = new PointF(0, 0);
        private ElevatorShaft location;
        private int floor;
        private Image imageDoorOpen;
        private Image imageDoorClosed;
        private Direction direction = Direction.NONE;
        private List<Human> occupants = new List<Human>();
        private List<Request> requests = new List<Request>();
        private double doorCounter = 0;
        // A variable to count up the milliseconds of the last update.
        private bool doorIsOpen;
        // In de code is de lift aangekomen, dit is het teken dat de deur open mag.
        private bool doorCanOpen;
        private const int hteDuration = 1000;
        // In geval van evacuatie sluit de lift af en gaat naar de begane grond.
        private float speed;
        private double hte;

        public Elevator(Facility specs)
        {
            location = specs.ElevatorShaft as ElevatorShaft;
            floor = 0;

            imageDoorOpen = Image.FromFile(@"../../Resources/elevator_opened2.png");
            imageDoorClosed = Image.FromFile(@"../../Resources/elevatoritself.png");
            Image = imageDoorClosed;
            this.hte = specs.Hte.WalkingSpeedTimeUnit;
            Dimension = new Point(1, 1);
        }

        /// <summary>
        /// Update the elevator. 
        /// </summary>
        /// <param name="updateTime">The milliseconds it took for the program to run the last update.</param>
        public void Update(double updateTime)
        {
            UpdateDoorState(updateTime);

            // Als de lift visueel aangekomen is op een andere verdieping, en de deur niet open is.
            if (position.Y == location.Position.Y * location.oneDimensionSize.Height && (!doorIsOpen))
            {
                // Als er requests zitten in de requestlijst en de deur is niet momenteel open, laat de lift dan bewegen.
                if (requests.Count != 0 && (!doorIsOpen))
                    Activate();
            }
            CalculateSpeed(updateTime);
            Move();
        }

        /// <summary>
        /// Laat gasten in en uitstappen.
        /// </summary>
        private void TransferGuests()
        {
            if (occupants.Count != 0)
                SortOutboundGuests();

            SortInboundGuests();
        }

        /// <summary>
        /// Determine the status of the door. 
        /// </summary>
        /// <param name="updateTime">The milliseconds it took for the program to run the last update.</param>
        private void UpdateDoorState(double updateTime)
        {
            // Als de lift op zowel in code als visueel op zijn plek gearriveerd is, open dan de deuren
            if ((doorCanOpen) && (position.Y == (location.Position.Y * oneDimensionSize.Height)))
            {
                // Open doors
                doorIsOpen = true;
                doorCanOpen = false;
            }

            // Zolang de deur open is.
            if (doorIsOpen)
            {
                TransferGuests();
                doorCounter += updateTime;

                if (Image != imageDoorOpen)
                    Image = imageDoorOpen;

                bool doorWillClose = (doorCounter >= (hteDuration * hte));

                // De deur gaat sluiten.
                if (doorWillClose)
                {
                    doorCounter = 0;
                    doorIsOpen = false;

                    Image = imageDoorClosed;
                }
            }
        }

        /// <summary>
        /// The parameter updateTime is used to calculate how much time has gone by. With this variable, the correct speed of the elevator can be determined.
        /// </summary>
        /// <param name="updateTime">The milliseconds it took for the program to run the last update.</param>
        private void CalculateSpeed(double updateTime)
        {
            speed = (((float)updateTime / hteDuration) * ((hteDuration / 100) * 40) / (float)hte);
        }

        /// <summary>
        /// Visually move the elevator on the screen.
        /// </summary>
        private void Move()
        {
            int destination = location.Position.Y * oneDimensionSize.Height;

            // preCalculatedMove berekent de volgende verplaatsing, als deze met enkele pixels zijn bestemming overschiet, wordt deze waarde gecorrigeerd.
            if (destination > position.Y)
            {
                float preCalculatedMove = position.Y + speed;

                if (preCalculatedMove > destination)
                {
                    float remainder = preCalculatedMove - (location.Position.Y * oneDimensionSize.Height);
                    position.Y = preCalculatedMove - remainder;
                }
                else
                {
                    position.Y += speed;
                }
            }
            else if (destination < position.Y)
            {
                float preCalculatedMove = position.Y - speed;

                if (preCalculatedMove < destination)
                {
                    float remainder = Math.Abs(preCalculatedMove - (location.Position.Y * oneDimensionSize.Height));
                    position.Y = preCalculatedMove + remainder;
                }
                else
                {
                    position.Y -= speed;
                }
            }
        }

        /// <summary>
        /// This method is tasked with handling requests from elevatorrooms troughout the building.
        /// </summary>
        /// <param name="request">Incoming request from an elevatorroom.</param>
        /// <returns>A variable to check whether the elevator rejects duplicate requests.</returns>
        public void HandleCall(Request request)
        {
            bool containsDuplicates = requests.Contains(request);
            // Als een request van dezelfde verdieping is en kan momenteel meegenomen worden, moet er geen "haal mij op" request geadd worden.
            bool skipRequest = (request.Floor == floor);

            // Als een request van dezelfde verdieping is, maar kan momenteel niet meegenomen worden, moet er toch een request geadd worden.
            if ((skipRequest) && (request.ButtonState != direction) && (direction != Direction.NONE))
                skipRequest = false;

            // If there are no duplicates requests, the request is not from the same floor where the elevator is, and there is no active firedrill.
            if ((!containsDuplicates) && (!skipRequest))
            {
                requests.Add(request);
                Sort();
            }

            if ((skipRequest) && ((request.ButtonState == direction) || (direction == Direction.NONE)))
                doorCanOpen = true;
        }

        /// <summary>
        /// Sort the requestlist according to the SCAN LOOK algorithm.
        /// How the algorithm works:
        /// 1. Sort the request list in 4 categories.
        /// Requests that go up or down and are in the path of the elevator current trajectory -
        /// - and requests that go up or down and are in the opposite direction of where the elevator is currently going.
        /// 2. Merge the sorted categories together in a way that is dependant on which direction the elevator is currently travelling.
        /// </summary>
        private void Sort()
        {
            bool firstAndOnlyRequest = (requests.Count == 1) && (direction == Direction.NONE);

            if (firstAndOnlyRequest)
            {
                if (requests[0].Floor > floor)
                    direction = Direction.UP;
                else if (requests[0].Floor < floor)
                    direction = Direction.DOWN;
            }
            else
            {
                List<Request> requestsUp = new List<Request>();
                List<Request> requestsDown = new List<Request>();
                List<Request> nextRequestsUp = new List<Request>();
                List<Request> nextRequestsDown = new List<Request>();

                foreach (Request request in requests)
                {
                    bool tagUp = (request.ButtonState == Direction.UP) || ((request.Floor > floor) && (request.ButtonState == Direction.NONE)); // Removing NONE from both these statements fixed Wikitest
                    bool tagDown = (request.ButtonState == Direction.DOWN) || (request.Floor < floor) && (request.ButtonState == Direction.NONE);

                    // Requests that want to go the same direction as the elevator, but are in the opposite direction
                    bool unsuitableUp = (request.ButtonState == Direction.UP && request.Floor < floor) && direction == Direction.UP;
                    bool unsuitableDown = (request.ButtonState == Direction.DOWN && request.Floor > floor) && direction == Direction.DOWN;

                    if (!unsuitableDown && !unsuitableUp)
                    {
                        if ((request.Floor < floor) && (direction == Direction.UP))
                            tagDown = true;
                        else if ((request.Floor > floor) && (direction == Direction.DOWN))
                            tagUp = true;

                        if (tagUp)
                            requestsUp.Add(request);
                        else if (tagDown)
                            requestsDown.Add(request);
                    }
                    else if (unsuitableUp)
                    {
                        nextRequestsUp.Add(request);
                    }
                    else if (unsuitableDown)
                    {
                        nextRequestsDown.Add(request);
                    }

                }
                requests.Clear();

                requestsUp = BubbleSort(requestsUp);
                requestsDown = BubbleSort(requestsDown);
                requestsDown.Reverse();

                nextRequestsUp = BubbleSort(nextRequestsUp);
                nextRequestsDown = BubbleSort(nextRequestsDown);
                nextRequestsDown.Reverse();

                if (direction == Direction.UP)
                {
                    requests.AddRange(requestsUp);
                    requests.AddRange(requestsDown);
                    requests.AddRange(nextRequestsUp);
                    requests.AddRange(nextRequestsDown);
                }
                else if (direction == Direction.DOWN)
                {
                    requests.AddRange(requestsDown);
                    requests.AddRange(requestsUp);
                    requests.AddRange(nextRequestsDown);
                    requests.AddRange(nextRequestsUp);
                }
            }
        }

        /// <summary>
        /// A simple bubblesort algorithm.
        /// Sorts requests in an ascending order (E.G. 1, 10)
        /// </summary>
        /// <param name="requestList">The list of requests that need to be sorted.</param>
        /// <returns>List of requests sorted with the bubblesort algorithm.</returns>
        private List<Request> BubbleSort(List<Request> requestList)
        {
            for (int i = 1; i < requestList.Count; i++)
            {
                for (int j = 0; j < (requestList.Count - i); j++)
                {
                    if (requestList[j].Floor > requestList[j + 1].Floor)
                    {
                        Request temp = requestList[j + 1];
                        requestList[j + 1] = requestList[j];
                        requestList[j] = temp;
                    }
                }
            }

            return requestList;
        }

        /// <summary>
        /// Allow the elevator to move one floor at a time.
        /// </summary>
        private void Activate()
        {
            int destination = requests.First().Floor;

            if (destination > floor)
                Ascend(destination);
            else if (destination < floor)
                Descend(destination);

            if (location.Position.Y == destination)
                Stop();
        }

        /// <summary>
        /// Halt the elevator at its destination. 
        /// </summary>
        /// <param name="destination">This floor.</param>
        private void Stop()
        {
            Direction tempDirection = requests[0].ButtonState;
            doorCanOpen = true;
            requests.RemoveAt(0);

            if (requests.Count == 0)
            {
                #region summary if01
                // Als er maar 1 request is en als die voltooid is, dan moet de lift naar idle mode gaan.
                // Direction mag NONE worden, de eerstvolgende request wordt door Sort() op een directie gezet.
                #endregion
                if (tempDirection == Direction.NONE)
                    direction = Direction.NONE;
                #region summary if02
                // Als de lift nog maar 1 request heeft ergens en die request is de tegenovergestelde richting in van de richting die lift nam om daar te komen, 
                // dan moet de lift wel omkeren als die gearriveerd is.
                #endregion
                else if (tempDirection != direction)
                    direction = tempDirection;
            }
            else if (requests.Count != 0)
            {
                if (requests[0].Floor == floor)
                {
                    if ((requests[0].ButtonState == Direction.NONE) && (requests[0].Floor == floor))
                    {
                        requests.RemoveAt(0);
                    }
                    #region summary if03
                    // Als de volgende request op dezelfde verdieping in, maar de omgekeerde richting ingaat
                    // Zet dan de status op NONE en verwijder deze request.
                    // Nu kan die persoon instappen en kan zijn request behandeld worden omdat de lift open staat voor requests door NONE
                    #endregion
                    else if ((requests[0].ButtonState != direction) && (tempDirection != direction))
                    {
                        direction = requests[0].ButtonState;
                        Debug.WriteLine("Removed {0}, {1}", requests[0].ButtonState, requests[0].Floor);
                        requests.RemoveAt(0);
                    }
                    #region summary if04 
                    // Als de volgende request op dezelfde verdieping in en dezelfde richting ingaat
                    // Verwijder dan alleen deze request, want dan stapt die guest zelf in om naar beneden te gaan
                    // En wordt de algoritme veilig gehouden
                    #endregion
                    else if (requests[0].ButtonState == direction)
                    {
                        Debug.WriteLine("Removed {0}, {1}", requests[0].ButtonState, requests[0].Floor);
                        requests.RemoveAt(0);
                    }
                }
                else
                {
                    if ((tempDirection != direction) && (tempDirection != Direction.NONE))
                        direction = tempDirection;
                }
            }

        }

        /// <summary>
        /// Move one floor up.
        /// </summary>
        /// <param name="destination">De volgende verdieping.</param>
        private void Ascend(int destination)
        {
            if (direction != Direction.UP)
                direction = Direction.UP;

            location = GetNeighbor(Direction.UP);
            floor++;
        }

        /// <summary>
        /// Move one floor down.
        /// </summary>
        /// <param name="destination">De volgende verdieping.</param>
        private void Descend(int destination)
        {
            if (direction != Direction.DOWN)
                direction = Direction.DOWN;

            location = GetNeighbor(Direction.DOWN);
            floor--;
        }

        /// <summary>
        /// Sort the persons who wish to enter on this floor.
        /// </summary>
        private void SortInboundGuests()
        {
            List<Human> incoming = location.SortInboundGuests(direction);

            if (incoming.Count != 0)
            {
                foreach (Human guest in incoming)
                {
                    occupants.Add(guest);
                    HandleCall(new Request(guest.Destination.Position.Y, Direction.NONE));
                }
            }
        }

        /// <summary>
        /// Sort out of all guests, the guests who wish to exit on this floor.
        /// </summary>
        private void SortOutboundGuests()
        {
            for (int i = (occupants.Count - 1); i > -1; i--)
            {
                if (occupants[i].Destination.Position.Y == floor)
                {
                    Human current = occupants[i];
                    occupants.Remove(current);
                    current.StepOutOfElevator(location.Position.Y, StatusHuman.WALK);
                }
            }
        }

        /// <summary>
        /// Finds the right neighbor in the desired direction.
        /// </summary>
        /// <param name="directionNeighbor">The direction in which we want to find a neighbor.</param>
        /// <returns>The requested neighbor.</returns>
        private ElevatorShaft GetNeighbor(Direction directionNeighbor)
        {
            ElevatorShaft neighbor = null;
            foreach (KeyValuePair<LocationType, int> item in location.Neighbor)
            {
                int neighborPosY = item.Key.Position.Y;
                int neighborPosX = item.Key.Position.X;

                if (directionNeighbor == Direction.UP)
                {
                    if (neighborPosY > location.Position.Y)
                        neighbor = item.Key as ElevatorShaft;
                }
                else if (directionNeighbor == Direction.DOWN)
                {
                    if (neighborPosY < location.Position.Y)
                        neighbor = item.Key as ElevatorShaft;
                }
                else if (directionNeighbor == Direction.LEFT)
                {
                    if (neighborPosX < location.Position.X)
                        neighbor = item.Key as ElevatorShaft;
                }
                else if (directionNeighbor == Direction.RIGHT)
                {
                    if (neighborPosX > location.Position.X)
                        neighbor = item.Key as ElevatorShaft;
                }
            }
            return neighbor;
        }

        /// <summary>
        /// Shut down the elevator in an emergency situation. 
        /// Return the elevator to the ground floor.
        /// </summary>
        public void EmergencyShutdown()
        {
            if (requests.Count > 0)
                requests.Clear();

            requests.Add(new Request(0, Direction.NONE));
        }

        /// <summary>
        /// Draw itself.
        /// </summary>
        /// <param name="Canvas">The graphical object.</param>
        /// <param name="WorldBitmap">The canvas on which the world is "painted".</param>
        public void Draw(Graphics Canvas, Bitmap WorldBitmap)
        {
            float posX = (location.Dimension.X * ((float)oneDimensionSize.Width / (float)1.55)); // lower is right 
            float posY = ((WorldBitmap.Height - this.position.Y) - this.oneDimensionSize.Height);
            float width = ((float)this.Dimension.X * (float)this.oneDimensionSize.Width / (float)2.85);
            float height = (float)this.Dimension.Y * this.oneDimensionSize.Height;

            Canvas.DrawImage(this.Image, posX, posY, width, height);
        }
    }
}
