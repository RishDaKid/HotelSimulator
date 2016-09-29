using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HotelSimulatie.Graph;
using HotelSimulatie.Actors;
using System.Drawing;

namespace HotelSimulatie.Facilities
{
    class Restaurant : Node
    {
        public int Capacity { get; set; }

        public Restaurant()
        {
            TileImage = Image.FromFile("../../Resources/restaurant.png");
        }
    }
}
