using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HotelSimulatie.Facilities;

namespace HotelSimulatie.Graph
{
    public class Node
    {
        public Dictionary<Node, int> Neighbors { get; set; }
        public Node Vorige { get; set; }
        public int afstand { get; set; }
        public LocationType LocationType { get; set; }

        public Node()
        {
            Neighbors = new Dictionary<Node, int>(); // voor de buren
            Vorige = null; // we hebben nog geen enkel pad gevonden
            afstand = Int32.MaxValue / 2; 
        }
    }
}
