using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HotelSimulatie.Facilities;
using HotelSimulatie.Graph;
namespace HotelSimulatie.Actors
{
    class Visitor : Human
    {
        public LocationType Location { get; set; }
        public LocationType Destination { get; set; }
        public Room Room { get; set; }
        public List<LocationType> Facilities { get; set; }
        public PathFinding pathFinding { get; set; }
        public VisitorGraph VisitorGraph { get; set; }

        public Visitor(List<LocationType> _facilities, PathFinding _pathFinding)
        {
            VisitorGraph = new VisitorGraph();
            pathFinding = _pathFinding;
            Facilities = _facilities;
            CreatePath();
        }

        public void CreatePath() // waar gast naar toe moet, hallwayroomb
        {
            // Heeft een locatie vanaf waar je wilt bewegen, onthoudt ook zijn locatie
            foreach (var item in Facilities)
            {
                if (item.Position.X == 1 && item.Position.Y == 0)
                {
                    Location = item;
                }
            }

            // Mag zelf bepalen waar die naar toe gaat, hoeft dus niet te onthoude
            foreach (var destination in Facilities)
            {
                if (destination.Position.X == 3 && destination.Position.Y == 1)
                {
                    VisitorGraph.CreateNodeGraph(Location, destination, Facilities);
                }
            }
        }
    }
}
