using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HotelSimulatie.Graph;
using HotelSimulatie.Actors;

namespace HotelSimulatie.Facilities
{
    class Cinema : Node
    {
        List<Visitor> CurrentWatching;
        public string AreaType { get; set; }
        public int Position { get; set; }
        public int Dimension { get; set; }


        public Cinema()
        {
            CurrentWatching = new List<Visitor>();


            //TileImage = Image.FromFile("../../Resources/background.png");
        }
    }
}
