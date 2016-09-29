using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HotelSimulatie.Graph;
using System.Drawing;

namespace HotelSimulatie.Facilities
{
    public class Room : Node
    {
        public string Classification { get; set; }
        public Room()
        {
            TileImage = Image.FromFile("../../Resources/room.png");
        }
    }
}

// Tony 28 sept 3:44