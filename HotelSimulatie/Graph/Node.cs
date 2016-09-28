using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelSimulatie.Graph
{
    public abstract class Node
    {
       // public Image TileImage { get; set; }
        public Dictionary<Node, int> Neighbors { get; set; }
        public Node Vorige { get; set; }
        public int afstand { get; set; } // afstand tot nu toe
        public string Naam { get; set; }

        public int Capacity { get; set; }
        public string AreaType { get; set; }
        public object Position { get; set; }
        public object Dimension { get; set; }
        public string Classification { get; set; }

        public Node()
        {
            Neighbors = new Dictionary<Node, int>(); // voor de buren
           // TileImage = Image.FromFile("../../Resources/background.png");

            Vorige = null; // we hebben nog geen enkel pad gevonden
            afstand = Int32.MaxValue / 2; // 
        }
    }
}
