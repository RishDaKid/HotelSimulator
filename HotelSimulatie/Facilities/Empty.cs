using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelSimulatie.Facilities
{
    class Empty : LocationType
    {
        public Empty()
        {
            Image = Image.FromFile("../../Resources/empty.png");
        }
    }
}
