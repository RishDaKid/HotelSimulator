using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HotelSimulatie.Actors;
using System.Drawing;

namespace HotelSimulatie.Facilities
{
    class Restaurant : LocationType
    {
        public int Capacity { get; set; }

        public Restaurant(int _capacity)
        {
            Capacity = _capacity;
            Image = Image.FromFile("../../Resources/restaurant.png");
        }
    }
}
