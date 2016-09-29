using HotelSimulatie.Actors;
using HotelSimulatie.Facilities;
using HotelSimulatie.Graph;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelSimulatie
{
    class Hotel
    {
        public List<Node> AssembleHotel()
        {
            #region Create objects from the list of model facilities
            FileReader fileReader = new FileReader();
            List<Facility> facilitiesModels = fileReader.ReadLayoutFile();


            List<Node> facilities = new List<Node>();
            List<Node> rooms = new List<Node>();

            // Haal alle faciliteiten uit de model list van faciliteiten 
            foreach (var item in facilitiesModels)
            {
                if (item.AreaType.Equals("Cinema"))
                {
                    Cinema cinema = new Cinema();
                    cinema.Position = item.Position;
                    cinema.Dimension = item.Dimension;
                    facilities.Add(cinema);
                }
                else if (item.AreaType.Equals("Restaurant"))
                {
                    Restaurant restaurant = new Restaurant();
                    restaurant.Capacity = item.Capacity;
                    restaurant.Position = item.Position;
                    restaurant.Dimension = item.Dimension;
                    facilities.Add(restaurant);
                }
                else if (item.AreaType.Equals("Room"))
                {
                    Room room = new Room();
                    room.Position = item.Position;
                    room.Dimension = item.Dimension;
                    room.Classification = item.Classification;
                    rooms.Add(room);
                }
                else
                {
                    Fitnesscentrum fitnessCentrum = new Fitnesscentrum();
                    fitnessCentrum.Position = item.Position;
                    fitnessCentrum.Dimension = item.Dimension;
                    facilities.Add(fitnessCentrum);
                }
            }
            #endregion

            #region Handmatig toevoegen hier:
            // 7, 5 is het eerste vakje rechtsonder
            Lobby lobby = new Lobby();
            Point point = new Point(7, 4);
            lobby.Position = point;
            facilities.Add(lobby);
            #endregion


            foreach (var item in rooms)
            {
                facilities.Add(item);
            }

            // Voeg de plekken die je handmatig hebt aangemaakt toe aan de lijst

            return facilities;
        }
    }
}
