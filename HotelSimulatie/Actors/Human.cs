using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HotelEvents;
using HotelSimulatie.Facilities;
using HotelSimulatie.Graph;
using System.Timers;
using HotelSimulatie.Model;
using HotelSimulatie.Enums;

namespace HotelSimulatie.Actors
{


    public abstract class Human : MovableCreature
    {
        // Human remembers his path so he can walk it and for more
        public List<Node> Path { get; protected set; }
        // Human remembers where his current location is right now
        public Node Location { get; protected set; }
        // This is the normal status what we use to mask out some fucntions
        public StatusHuman StatusHuman { get; protected set; } = StatusHuman.NONE;
        // We save this so human remembers what he is doing
        public HotelEvent CurrentEvent { get; protected set; }
        // visitor remembers the pathfinding
        public PathFinding PathFinding { get; protected set; }
        // remembers it's destination because it needs to know if and when he is on his destination for some logic coding
        public LocationType Destination { get;  set; }
        // To calculate which step human has to take
        public int NextStep { get; protected set; } = 1;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="humanParam"></param>
        public Human(MovableParam humanParam)
        {
            // Defining properties / fields
            Settings = humanParam.Settings;
            PathFinding = humanParam.PathFinding;
            Xpos = 0;
            Ypos = 0;
            Size = new Size(20, 50);
            Location = new Node();
            CurrentEvent = new HotelEvent();
            CurrentEvent.EventType = HotelEventType.NONE;
        }

        /// <summary>
        /// Updating all humans. They can give their own implementation
        /// </summary>
        /// <param name="drawUpdateTime"></param>
        public abstract void Update(double drawUpdateTime);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="newLocationY"></param>
        /// <param name="status"></param>
        public void StepOutOfElevator(int newLocationY, StatusHuman status)
        {
            Visible = true;
            StatusHuman = status;
            int distanceTravelled = Math.Abs(newLocationY - Location.LocationType.Position.Y);
            NextStep += (distanceTravelled - 1);
            Location = Path[NextStep];
            Ypos = (Location.LocationType.Position.Y * 80);
        }

        /// <summary>
        /// Get event from notifyer in Hotel. We save it as the CurrentEvent. You have to see that as the brain of cleaner, what he wants to do.
        /// </summary>
        /// <param name="currentEvent"></param>
        public void GetCurrentEvent(HotelEvent _currentEvent)
        {
            // When cleaner is alive
            if (Alive == true)
            {
                if (CurrentEvent.EventType != HotelEventType.CHECK_IN)
                {
                    // When cleaner it's status is not evacuate
                    if (StatusHuman != StatusHuman.EVACUATE && (Visible) && StatusHuman != StatusHuman.WAITING || _currentEvent.EventType == HotelEventType.EVACUATE)
                    {
                        CurrentEvent = _currentEvent;
                    }
                }
            }
        }

        /// <summary>
        /// Setting human position, status and interacting
        /// </summary>
        /// <param name="drawUpdateTime"></param>
        public override void WalkCheck(double drawUpdateTime)
        {
            if (Path != null && this.Location.LocationType != null)
            {
                if (StatusHuman != StatusHuman.INTERACTING && StatusHuman != StatusHuman.WAITING)
                {
                    // How many updates i need before i get 1 second, based on that i set the walkingspeed
                    double walkingSpeedOnHTE = 0;
                    // walk to destination
                    if (NextStep < Path.Count)
                    {
                        // Check if xPos is smaller the position of the next one
                        if (Xpos < (((Path[NextStep].LocationType.Position.X)) * Path[NextStep].LocationType.oneDimensionSize.Width))
                        {
                            walkingSpeedOnHTE = drawUpdateTime / OneHTE * WalkingDistancePerHte;
                            WalkingSpeed = walkingSpeedOnHTE / Settings.WalkingSpeedTimeUnit;
                            if ((Path[NextStep].LocationType.Position.X * Path[NextStep].LocationType.oneDimensionSize.Width) < Xpos + WalkingSpeed)
                            {
                                WalkingSpeed = (Path[NextStep].LocationType.Position.X * Path[NextStep].LocationType.oneDimensionSize.Width) - Xpos;
                            }
                            WalkingStatus = WalkingStatus.RIGHT;
                        } // 50 ms / 1000 * 400 = 20; 20 pixels per keer en dat 20 updates. dus 400 pixels in 1 sec/hte.

                        //// Check if xPos is bigger the position of the next one
                        else if (Xpos > (Path[NextStep].LocationType.Position.X) * Path[NextStep].LocationType.oneDimensionSize.Width)
                        {
                            walkingSpeedOnHTE = drawUpdateTime / OneHTE * WalkingDistancePerHte;
                            WalkingSpeed = walkingSpeedOnHTE / Settings.WalkingSpeedTimeUnit;
                            if ((Path[NextStep].LocationType.Position.X * Path[NextStep].LocationType.oneDimensionSize.Width) > Xpos - WalkingSpeed)
                            {
                                WalkingSpeed = Xpos - (Path[NextStep].LocationType.Position.X * Path[NextStep].LocationType.oneDimensionSize.Width);
                            }
                            WalkingStatus = WalkingStatus.LEFT;
                        }
                        // Check if yPos is smaller the position of the next one
                        else if (Ypos < (Path[NextStep].LocationType.Position.Y) * Path[NextStep].LocationType.oneDimensionSize.Height)
                        {
                            walkingSpeedOnHTE = drawUpdateTime / OneHTE * WalkingDistancePerHte;
                            WalkingSpeed = walkingSpeedOnHTE / Settings.WalkingSpeedTimeUnit;
                            if ((Path[NextStep].LocationType.Position.Y * Path[NextStep].LocationType.oneDimensionSize.Height) < Ypos + WalkingSpeed)
                            {
                                WalkingSpeed = (Path[NextStep].LocationType.Position.Y * Path[NextStep].LocationType.oneDimensionSize.Height) - Ypos;
                            }
                            WalkingStatus = WalkingStatus.UP;
                        }
                        // Check if yPos is bigger the position of the next one
                        else if (Ypos > (Path[NextStep].LocationType.Position.Y) * Path[NextStep].LocationType.oneDimensionSize.Height)
                        {
                            walkingSpeedOnHTE = drawUpdateTime / OneHTE * WalkingDistancePerHte;
                            WalkingSpeed = walkingSpeedOnHTE / Settings.WalkingSpeedTimeUnit;
                            if ((Path[NextStep].LocationType.Position.Y * Path[NextStep].LocationType.oneDimensionSize.Height) > Ypos - WalkingSpeed)
                            {
                                WalkingSpeed = Ypos - (Path[NextStep].LocationType.Position.Y * Path[NextStep].LocationType.oneDimensionSize.Height);
                            }
                            WalkingStatus = WalkingStatus.DOWN;
                        }
                        Move();

                        #region Not finished function
                        //// When human is left under a facility
                        //if (xPos >= (path[nextStep].LocationType.Position.X) * path[nextStep].LocationType.OneDimensionWidth && xPos <= (path[nextStep].LocationType.Position.X + 1) * path[nextStep].LocationType.OneDimensionWidth && yPos == ((path[nextStep].LocationType.Position.Y) * path[nextStep].LocationType.OneDimensionHeight))
                        //{
                        //    if (this.Location.LocationType is ElevatorShaft || Location.LocationType is Staircase)
                        //    {
                        //        walkingspeed = 2;
                        //    }
                        //    else
                        //    {
                        //        walkingspeed = 5;
                        //    }
                        //}
                        #endregion

                        // When a guest arrives at a new square
                        if (Xpos == (Path[NextStep].LocationType.Position.X) * Path[NextStep].LocationType.oneDimensionSize.Width && Ypos == ((Path[NextStep].LocationType.Position.Y) * Path[NextStep].LocationType.oneDimensionSize.Height))
                        {
                            WalkingStatus = WalkingStatus.STANDSTILL;
                            this.Location.SetLocationType(Path[NextStep].LocationType);
                            NextStep++;

                            if ((this.Location.LocationType is ElevatorShaft) && (Destination.Position.Y != Location.LocationType.Position.Y))
                            {
                                this.Location.LocationType.Interact(this);

                                StatusHuman = StatusHuman.WAITING;
                            }
                        }
                    }
                    // interact with destination and walk to where visitor moved from
                    else if (this.Location.LocationType == Destination)
                    {
                        // so visitor can walk back to his room
                        NextStep = 1;
                        // Human interacts with facility
                        this.Location.LocationType.Interact(this);
                        // Sets it's status on interacting
                        StatusHuman = StatusHuman.INTERACTING;
                    }
                }
            }
        }

        /// <summary>
        /// Human setting it's own walking status
        /// </summary>
        public abstract void SetWalkingStatus();
    }
}
