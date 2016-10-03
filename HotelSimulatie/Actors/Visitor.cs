using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HotelSimulatie.Facilities;

namespace HotelSimulatie.Actors
{
    class Visitor : Human
    {
        public LocationType location;
        public Room room;

        //public string CreatePath(Node _destination) // waar gast naar toe moet, hallwayroomb
        //{
        //    CopyNode startLocation = new CopyNode(location); // rooma
        //    startLocation.afstand = 0;
        //    string path = startLocation.Dijkstra(startLocation, _destination);
        //    return path;
        //}
    }
}
