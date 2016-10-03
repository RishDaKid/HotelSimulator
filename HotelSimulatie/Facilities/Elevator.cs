using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HotelSimulatie.Actors;
using System.Drawing;

namespace HotelSimulatie.Facilities
{
    public class Elevator : LocationType
    {
        List<Visitor> Guests;

        public Elevator()
        {
            Guests = new List<Visitor>();
            Image = Image.FromFile("../../Resources/elevator.png");

        }
    }
}
