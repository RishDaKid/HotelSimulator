using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelSimulatie.Graph
{
    class DijkstraNode : Node
    {
        private Node Source;
        private Node previous;

        private int afstand;
        private Dictionary<DijkstraNode, int> DijkstraNeighbors;
        public List<DijkstraNode> open;


        public DijkstraNode(Node _source)
        {
            Source = _source;
            afstand = Int32.MaxValue / 2;
            DijkstraNeighbors = new Dictionary<DijkstraNode, int>();
            open = new List<DijkstraNode>();
            Naam = Source.Naam;

            foreach (KeyValuePair<Node, int> n in Source.Neighbors)
            {
                Neighbors.Add(new DijkstraNode(n.Key), n.Value);
            }
        }
    }
}
