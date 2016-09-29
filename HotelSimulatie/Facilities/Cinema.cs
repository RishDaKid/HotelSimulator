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
    class Cinema : Node
    {

        public Cinema()
        {
            TileImage = Image.FromFile("../../Resources/cinema.png");
        }
    }
}
