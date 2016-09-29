using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HotelSimulatie.Graph;
using System.Drawing;

namespace HotelSimulatie.Facilities
{
    class Elevator : Node
    {
        public Elevator()
        {
            TileImage = Image.FromFile("../../Resources/elevator.png");
        }
    }
}

// class gemaakt. die noem je dijkstra node. Die krijgt bij het aanmaken een node. je maakt int afstand. dictionarry buren. buren = new dictionarry. foreach : key.value.