using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HotelSimulatie.Factory;

namespace HotelSimulatie.Facilities
{
    public abstract class LocationType 
    {

        public Image Image { get; set; }
        public Point Position { get; set; }
        public Point Dimension { get; set; }
        public string AreaType { get; set; }
        public int Height { get; set; }
        public int Width { get; set; }
        public Dictionary<LocationType, int> neighBor;

        public LocationType()
        {
            neighBor = new Dictionary<LocationType, int>();
            Height = 120;
            Width = 160;
        }

    }
}
