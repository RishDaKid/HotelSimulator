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
    class Staircase : Node
    {
        List<Visitor> Guests;
        public Staircase()
        {
            Guests = new List<Visitor>();

            TileImage = Image.FromFile("../../Resources/lobby.png");
        }
    }
}
