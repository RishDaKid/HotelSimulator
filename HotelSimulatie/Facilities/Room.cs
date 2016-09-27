using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HotelSimulatie.Graph;

namespace HotelSimulatie.Facilities
{
    public class Room : Node
    {
        public string Classification  { get; set; }
        public string AreaType { get; set; }
        public int Position { get; set; }
        public int Dimention { get; set; }


    }
}
