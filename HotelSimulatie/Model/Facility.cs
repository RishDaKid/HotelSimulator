using HotelSimulatie.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelSimulatie.Facilities
{
    public class Facility
    {
        // We will create models from the file objects
        public int Capacity { get; set; }
        public string AreaType { get; set; }
        public Point Position { get; set; }
        public string Classification { get; set; }
        public Point Dimension { get; set; }
        public int ID { get; set; }
        public LocationType ElevatorShaft { get; set; }
        public Settings Hte { get; set; }
    }
}
