using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HotelSimulatie.Factory;
using HotelSimulatie.Graph;
using HotelSimulatie.Actors;
using System.Timers;
using HotelSimulatie.Model;
using HotelSimulatie.Enums;

namespace HotelSimulatie.Facilities
{
    public abstract class LocationType : CreatableObject
    {
        // Every areatype has an ID so we can choose which one of the same type we want
        public int ID { get; private set; }
        // Every locationtype has an Areatype
        public string AreaType { get; private set; }
        // Every locationtype has a couple of neighbors
        public Dictionary<LocationType, int> Neighbor { get; private set; }
        // When this locationtype has to be claened, we set it on true
        public bool NoNeedToBeCleaned { get; private set; } = true;
        // We do not say that this list is a list of visitors, maybe later on their can be other humans who can interact with facilities aswell
        public List<Human> CurrentHumans { get; private set; }
        // Status if facility is open/closed
        public OpenClosedStatus InteractingStatus { get; private set; } = OpenClosedStatus.OPEN;
        // We make a instance of the settings class where we will read the settinf file on
        protected Settings HTE;
        // Visitor allways cleans 100 integer worth of garbage
        protected double OneHTE = 1000;
        // Queue for the entrace
        public Queue<Human> EntranceQueue { get; private set; }
        // Position of every areatype
        public Point Position { get; protected set; }

        // Constructor
        public LocationType(Facility specs)
        {
            // Define all properties and none properties
            EntranceQueue = new Queue<Human>();
            AreaType = specs.AreaType;
            SetPosition(specs.Position);
            Dimension = specs.Dimension;
            ID = specs.ID;
            HTE = specs.Hte;
            CurrentHumans = new List<Human>();
            Neighbor = new Dictionary<LocationType, int>();
        }

        /// <summary>
        /// Adding neighbor to this object
        /// </summary>
        /// <param name="neighBor"></param>
        public void SetNeighbor(LocationType neighBor, int weight)
        {
            Neighbor.Add(neighBor, weight);
        }

        /// <summary>
        /// Setting the position of this locationtyp dynamically
        /// </summary>
        /// <param name="position"></param>
        public void SetPosition(Point position)
        {
            Position = position;
        }

        /// <summary>
        /// When facility get's attacked 
        /// </summary>
        /// <returns></returns>
        public bool Attacked()
        {
            // Check if the Dimension is bigger than one
            if (Dimension.X > 1)
            {
                Dimension = new Point(Dimension.X - 1, Dimension.Y);
            }
            // Or else return true to delete this facility. There is not point in setting the Dimension to 0 and remove it afther that.
            else
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Locationtype is being notified when Hotel get's attacked
        /// Locationtype it self has to find out if it is he who's getting attacked by comparing if the is the most right side of the hotel
        /// </summary>
        /// <param name="statusHotel"></param>
        /// <param name="positionX"></param>
        /// <param name="DimensionX"></param>
        /// <returns></returns>
        public bool Notify(HotelEvents.HotelEventType statusHotel, int positionX, int DimensionX)
        {
            if (statusHotel == HotelEvents.HotelEventType.GODZILLA)
            {
                if ((Position.X + Dimension.X).Equals((positionX + DimensionX)))
                {
                    if (Attacked() == true)
                    {
                        return true;
                    }
                }
            }
            return false;
        }


        /// <summary>
        /// Set cleaning status
        /// </summary>
        /// <param name="_cleaningStatus"></param>
        public void CleaningStatus(bool cleaningStatus)
        {
            // Cleaner will tell when he is done with cleaning, so this areatype will set it's status on not being cleaned
            NoNeedToBeCleaned = cleaningStatus;
        }

        /// <summary>
        /// Every facility has a timer(update), for handeling events and checking purposes
        /// </summary>
        /// <param name="drawUpdateTime">Use this as a timer when you need it because we use the gameloop to handle things</param>
        public abstract void Update(double drawUpdateTime);

        /// <summary>
        /// // With every facility it is possible to interact, but every facility gives has an other behaviour, thus other implementation
        /// </summary>
        /// <param name="human">Human can interact with this class, give them the opportunity to</param>
        public abstract void Interact(Human human);

        /// <summary>
        /// When human want's to interact with the facility, he will first join the facility
        /// </summary>
        /// <param name="human"></param>
        public void JoinFacility(Human human)
        {
            CurrentHumans.Add(human);
        }

        /// <summary>
        /// People will leave facility when they are done interacting
        /// </summary>
        /// <param name="human"></param>
        public void LeaveFacility(Human human)
        {
            CurrentHumans.Remove(human);
        }

        /// <summary>
        /// Clean this facility
        /// </summary>
        /// <param name="drawUpdateTime"></param>
        /// <returns></returns>
        public bool CleanFacility(double drawUpdateTime)
        {
            // cleaning
            OneHTE -= (int)drawUpdateTime / HTE.CleaningTimeUnit;
            if(CheckIfClean() == true)
            {
                // when cleaning is done
                return true;
            }
            // when cleaning is not over
            return false;
        }

        /// <summary>
        /// Check if facility is cleaned then return true so cleaner can leave the facility
        /// </summary>
        /// <returns></returns>
        public bool CheckIfClean()
        {
            // When areatype is cleaned
            if (OneHTE < 1)
            {
                // Set status on NoNeedToBeCleaned
                CleaningStatus(true);
                // There is always 10, to be cleaned
                OneHTE = 1000;
                return true;
            }
            // when cleaning is not over
            return false;
        }

        /// <summary>
        /// We let all the movable creatures draw it self
        /// </summary>
        /// <param name="Canvas"></param>
        /// <param name="WorldBitmap"></param>
        public void Draw(Graphics Canvas, Bitmap WorldBitmap)
        {
            Canvas.DrawImage(Image, (Position.X) * oneDimensionSize.Width, WorldBitmap.Height - (Position.Y * oneDimensionSize.Height) - Dimension.Y * oneDimensionSize.Height, Dimension.X * oneDimensionSize.Width, Dimension.Y * oneDimensionSize.Height);
        }
    }
}
