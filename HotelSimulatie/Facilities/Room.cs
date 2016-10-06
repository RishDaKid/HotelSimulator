using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace HotelSimulatie.Facilities
{
    public class Room : LocationType
    {
        public string Classification { get; set; }
        public Room(string _classification)
        {
            Classification = _classification;
            Image = Image.FromFile("../../Resources/room.png");
        }
    }
}

// Tony 28 sept 3:44