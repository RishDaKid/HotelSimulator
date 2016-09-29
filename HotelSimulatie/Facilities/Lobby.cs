using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HotelSimulatie.Graph;
using System.Drawing;

namespace HotelSimulatie.Facilities
{
    class Lobby : Node
    {

        public Lobby()
        {
            //List<Human> // Hangt af of we een rij nodig hebben
            TileImage = Image.FromFile("../../Resources/lobby.png");
        }
    }
}
