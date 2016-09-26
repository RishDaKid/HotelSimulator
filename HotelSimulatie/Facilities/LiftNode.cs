using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HotelSimulatie.Actors;
using HotelSimulatie.Graph;

namespace HotelSimulatie.Facilities
{
    public class LiftNode : Node
    {
        List<Visitor> Guests;

        public LiftNode()
        {
            Guests = new List<Visitor>();
            //TileImage = Image.FromFile("../../Resources/background.png");
        }
    }
}
