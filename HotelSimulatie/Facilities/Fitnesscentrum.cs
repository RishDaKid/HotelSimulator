using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HotelSimulatie.Graph;
using System.Drawing;

namespace HotelSimulatie.Facilities
{
    class Fitnesscentrum : Node
    {
        public Fitnesscentrum()
        {
            TileImage = Image.FromFile("../../Resources/fitness.png");
        }
    }
}
