using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using HotelSimulatie.Model;
using HotelSimulatie.Graph;
using System.Diagnostics;
using HotelSimulatie.Facilities;

namespace HotelSimulatie
{
    class FileReader
    {
        public List<Node> FacilitiesFromFile{ get; set; }
        public List<Node> RoomsFromFile { get; set; }

        JsonSerializer jsonSerializer;

        public FileReader()
        {
            FacilitiesFromFile =  new List<Node>();
            RoomsFromFile =  new List<Node>();
            jsonSerializer = new JsonSerializer();
        }

        public void ReadLayoutFile()
        {
            string text = File.ReadAllText(@"../../Hotel2.layout");
            List<Facility> Facilities = JsonConvert.DeserializeObject<List<Facility>>(text);

            // Haal alle faciliteiten uit de model list van faciliteiten 
            foreach (var item in Facilities)
            {
                if (item.AreaType.Equals("Cinema"))
                {
                    Cinema cinema = new Cinema();
                    cinema.Position = item.Position;
                    cinema.Dimension = item.Dimension;
                    FacilitiesFromFile.Add(cinema);
                }
                else if (item.AreaType.Equals("Restaurant"))
                {
                    Restaurant restaurant = new Restaurant();
                    restaurant.Capacity = item.Capacity;
                    restaurant.Position = item.Position;
                    restaurant.Dimension = item.Dimension;
                    FacilitiesFromFile.Add(restaurant);
                }
                else if (item.AreaType.Equals("Room"))
                {
                    Room room = new Room();
                    room.Position = item.Position;
                    room.Dimension = item.Dimension;
                    room.Classification = item.Classification;
                    RoomsFromFile.Add(room);
                }
                else
                {
                    Fitnesscentrum fitnessCentrum = new Fitnesscentrum();
                    fitnessCentrum.Position = item.Position;
                    fitnessCentrum.Dimension = item.Dimension;
                    FacilitiesFromFile.Add(fitnessCentrum);
                }
            }
            //return FacilitiesFromFile;
            //return RoomsFromFile;
        }

        public Settings GetSettings()
        {
            string text = File.ReadAllText(@"../../settings.json");
            Settings settings = JsonConvert.DeserializeObject<Settings>(text);
            //Console.WriteLine(settings.CinemaTimeUnit + " " + settings.RestaurantTimeUnit + " " + settings.StairsTimeUnit + " " + settings.DeathTimeUnit + " " + settings.CleaningTimeUnit + " " + settings.CleaningEmergengyTimeUnit);
            return settings;
        }

        public void WriteFile(Settings settings)
        {
            StreamWriter sw = new StreamWriter(@"../../settings.json");
            jsonSerializer.Serialize(sw, settings);
            sw.Close();
        }
    }
}

// 28 SEPT
// http://stackoverflow.com/questions/15368231/can-json-numbers-be-quoted