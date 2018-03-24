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
        // Values
        public Dictionary<Node, int> Neighbors { get; private set; }
        public Node Previous { get; private set; }
        public int Distance { get; private set; }
        public LocationType LocationType { get; private set; }

        /// <summary>
        /// Consturctor
        /// </summary>
        public Node()
        {
            Neighbors = new Dictionary<Node, int>(); // voor de buren
            Previous = null; // We hebben nog geen enkel pad gevonden
            Distance = Int32.MaxValue / 2; 
        }

        /// <summary>
        /// Setting it's distance
        /// </summary>
        /// <param name="distance"></param>
        public void SetDistance(int distance)
        {
            Distance = distance;
        }

        /// <summary>
        /// Set previous node 
        /// </summary>
        /// <param name="_previous"></param>
        public void SetPrevious(Node previous)
        {
            Previous = previous;
        }

        /// <summary>
        /// Setting it's own neighbour
        /// </summary>
        /// <param name="nextNode"></param>
        /// <param name="distance"></param>
        public void SetNeighbour(Node nextNode, int distance)
        {
            Neighbors.Add(nextNode, distance);
        }

        /// <summary>
        /// Set locationtype os this node
        /// </summary>
        /// <param name="_locationType"></param>
        public void SetLocationType(LocationType locationType)
        {
            LocationType = locationType;
        }
    }
}
