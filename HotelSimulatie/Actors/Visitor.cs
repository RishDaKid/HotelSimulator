using HotelSimulatie.Facilities;
using HotelSimulatie.Graph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelSimulatie.Actors
{
    class Visitor : Human
    {
        public Node location;
        public Room room;

        public Visitor()
        {

        }

        public string createPath(Node _destination)
        {
            CopyNode start = new CopyNode(location);
            start.Afstand = 0;
            string path = start.Dijkstra(start, _destination);
            return path;
        }
    }
}
