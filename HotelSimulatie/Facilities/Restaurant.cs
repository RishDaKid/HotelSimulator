using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HotelSimulatie.Actors;
using System.Drawing;
using System.Timers;

namespace HotelSimulatie.Facilities
{
    class Restaurant : LocationType
    {
        // Restaurant has an capacity
        public int Capacity { get; set; }

        public Restaurant(Facility specs) : base(specs)
        {
            Image = Image.FromFile("../../Resources/restaurant.png");
            Capacity = specs.Capacity;
        }

        /// <summary>
        /// interact with visitor
        /// </summary>
        /// <param name="human"></param>
        public override void Interact(Human human)
        {
            JoinFacility(human);
        }

        /// <summary>
        /// Updating this object based on the gameloop
        /// </summary>
        /// <param name="drawUpdateTime"></param>
        public override void Update(double drawUpdateTime)
        {
        }
    }
}
