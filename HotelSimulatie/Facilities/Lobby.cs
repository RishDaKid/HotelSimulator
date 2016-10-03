using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace HotelSimulatie.Facilities
{
    class Lobby : LocationType
    {

        public Lobby()
        {
            //List<Human> // Hangt af of we een rij nodig hebben
            Image = Image.FromFile("../../Resources/Lobbyy.png");
        }
    }
}
