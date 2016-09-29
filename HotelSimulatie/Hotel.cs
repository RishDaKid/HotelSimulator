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
        private FileReader fileReader;


        public Hotel()
        {
            fileReader = new FileReader();
            creatHotel();
        }

        public List<Node> creatHotel()
        {
            fileReader.ReadLayoutFile();

            // Hotel 
            List<Node> Hotel = new List<Node>();



            // Lobby
            Lobby lobby = new Lobby();

            // Facilities
            List<Node> Facilities = fileReader.FacilitiesFromFile;

            // rooms
            List<Node> rooms = fileReader.RoomsFromFile;



            // Place Facilities in Hotel
            foreach (var item in rooms)
            {
                Hotel.Add(item);
            }

            // Place Rooms in Hotel
            foreach (var item in Facilities)
            {
                Hotel.Add(item);
            }

            // Place Lobby in Hotel
            Hotel.Add(lobby);

            return Hotel;
        }
    }
}
