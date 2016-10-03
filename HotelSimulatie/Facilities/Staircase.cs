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
        List<Visitor> Guests;
        public Staircase()
        {
            Guests = new List<Visitor>();

            Image = Image.FromFile("../../Resources/staircase.png");
        }
    }
}
