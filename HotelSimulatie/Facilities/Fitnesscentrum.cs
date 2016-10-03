using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace HotelSimulatie.Facilities
{
    class Fitnesscentrum : LocationType
    {
        public Fitnesscentrum()
        {
            Image = Image.FromFile("../../Resources/FitnessCentrum.png");
        }
    }
}
