using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HotelSimulatie.Graph;
using HotelSimulatie.Facilities;

namespace HotelSimulatie.Actors
{
    class Visitor : Human
    {
        public Node location;
        public Room room;

        public string createPath(Node _destination) // waar gast naar toe moet, hallwayroomb
        {
            CopyNode start = new CopyNode(location); // rooma
            start.afstand = 0;
            string path = start.Dijkstra(start, _destination);
            return path;
        }
    }
}
