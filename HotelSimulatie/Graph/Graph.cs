using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelSimulatie.Graph
{
    public class DGraph
    {
        // we bezoeken de knoop deze, tenzij die eind is.
        // vervolgens veranderen we deze de kortste knoop tot nu toe en dan zoeken we netzolang totdat het einde is en dan zijn we klaar.
        List<Node> open = new List<Node>();
        public string Dijkstra(Node begin, Node eind)
        {
            Node deze = begin; // waar mee je begint
            while (!Bezoek(deze, eind)) // zolang we niet de eindknoop bezoeken, bezoeken we dus deze, oftewel zolang deze niet de eindknoop is en we gaan hem zoeken gaan we door. Doorgaan betekend : we veranderen deze in de kortste knoop die we tot nu toe hebben.
            {
                // pak het to nu toe kosrste pad, eerste a + tweede a, previous + derde a, previus plus vierde a, previous plus  vijfde a.
                deze = open.Aggregate((l, r) => l.afstand < r.afstand ? l : r); // we maken deze gelijk aan de knoop in de collectie open die de kleinste afstand heeft. Dat is precies wat we wilde, Dijkstra zij pak de knoop met de tot nu toe de kortste pad en die gaan we bezoeken. 
            }
            return MakePath(eind); // die maakt die string waarbij die zegt we hebben de pad gevonden en dan moet je die juiste knopen in de juiste volgorde in dat stringetje zetten om op scherm te zetten.
        }
        public string MakePath(Node End)
        {
            List<string> stappenPad = new List<string>();
            Node vorige = End;
            string deelPad = null;
            string volledigPad = null;
            while (vorige != null)
            {
                deelPad = vorige.Naam;
                stappenPad.Add(deelPad);
                vorige = vorige.Vorige;
            }
            stappenPad.Reverse();
            foreach (var item in stappenPad)
            {
                volledigPad += (item + " ");
            }
            return volledigPad;
        }

        public bool Bezoek(Node deze, Node eind) // knoop deze, de noop die we willen bezoeken en de knoop eind.
        {
            Console.WriteLine("Ik bezoek knoop : " + deze.Naam);
            // checken op eind, als de knoop deze gelij kis aan de eindknoop. Als we true returnen in de whileloop dan zijn we klaar. Dan kunnen we het korste pad maken en zometeen afdrukken
            if (deze == eind)
            {
                return true;
            }
            // niet meer bezoeken
            if (open.Contains(deze)) // als we de knoop deze bezoeken moeten dan we hem uit het lijstje halen van de nog te bezoeken knopen. Als deze erin zit moeten we hem eruit halen. 
            {
                open.Remove(deze);
            }
            // Buren aflopen
            foreach (KeyValuePair<Node, int> x in deze.Neighbors) // Buren van elk knoop aflopen, in dit geval van deze. Dus die halen we eruit.
            {
                int nieuweAfstand = deze.afstand + x.Value; // Afstand dat we tot nu toe hebben + waarde die je nodig hebt om tot die knoop te komen.
                if (nieuweAfstand < x.Key.afstand) // Als dat korter is dan de afstand die we eventueel al hadden berekend voor x, dan moeten we hem aanpassen. Als de nieuwe afstand die we berekend hebben via de knoop deze, als dat sneller is dan de afstand die x tot nu toe(in hetb egin was die oneindig dus het was altijd zo) had dan moeten we die afstand aanpassen.
                {
                    x.Key.afstand = nieuweAfstand; // Nieuwe afstand zetten als sneller is
                    x.Key.Vorige = deze; // Als we een nieuwe afstand moeten berekenen betekend het dat de route via deze naar zijn kind knoop x de snelste route tot nu toe is, en dat willen we onthouden. Dus op dit moment onthouden we van knoop x dat we de snelste route daarna via de knop deze was. Van de knopp x, zijn vorige wordt gelijk aan deze.
                    open.Add(x.Key); // Uiteindelijk moeten we de knoop x nog kunnen bezoeken dus voegen we het toe aan het lijstje zodat als dit tot nu de kortste pad is we die ook kunnen bezoeken.
                }
            }
            return false; // Zodat de whileloop niet eindigd. Bij true wel maar we moeten er nog doorheen.
        }
    }
}
