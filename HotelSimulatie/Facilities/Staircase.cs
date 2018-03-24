using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HotelSimulatie.Actors;
using System.Drawing;

namespace HotelSimulatie.Facilities
{
    class Staircase : LocationType
    {
        public Staircase(Facility specs) : base(specs)
        {
            Image = Image.FromFile("../../Resources/stairs.png");

        }

        /// <summary>
        /// If we want human to interact with the stairs
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
