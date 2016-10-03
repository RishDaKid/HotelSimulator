using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HotelSimulatie.Actors;
using System.Drawing;

namespace HotelSimulatie.Facilities
{
    class Cinema : LocationType
    {

        public Cinema()
        {
            Image = Image.FromFile("../../Resources/cinema.png");
        }
    }
}
