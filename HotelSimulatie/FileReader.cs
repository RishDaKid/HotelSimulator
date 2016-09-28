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
        JsonSerializer jsonSerializer;

        public FileReader()
        {
            jsonSerializer = new JsonSerializer();
        }

        public Settings ReadLayout()
        {
            string text = File.ReadAllText(@"../../Hotel2.layout");
            // List with "models" not yet converted 
            List<Facility> Facilities = JsonConvert.DeserializeObject<List<Facility>>(text);

            var obj = JsonConvert.DeserializeObject<dynamic>(text);

            //Console.WriteLine(obj[0]["Dimension"].ToString());
            Console.WriteLine(obj[0]["Dimension"].GetType());

            foreach (var item in Facilities)
            {
                Debug.WriteLine(item);

            }

            // http://stackoverflow.com/questions/15368231/can-json-numbers-be-quoted

            List<Node> facs =  new List<Node>();
            foreach (var item in Facilities)
            {
                //Console.WriteLine(item.Capacity.GetType());
                switch (item.AreaType)
                {
                    case "Cinema":
                        Cinema cinema = new Cinema();
                        cinema.Position = item.Position;
                        cinema.Dimension = item.Dimension;
                        facs.Add(cinema);
                        break;
                    case "Restaurant":
                        Restaurant restaurant = new Restaurant();
                        restaurant.Capacity = item.Capacity;
                        restaurant.Position = item.Position;
                        restaurant.Dimension = item.Dimension;
                        facs.Add(restaurant);
                            
                        break;
                    case "Fitness":
                        Fitnesscentrum fitnessCentrum = new Fitnesscentrum();
                        fitnessCentrum.Position = item.Position;
                        fitnessCentrum.Dimension = item.Dimension;
                        facs.Add(fitnessCentrum);
                        break;
                    case "Room":
                        Room room = new Room();
                        room.Classification = item.Classification;
                        room.Position = item.Position;
                        room.Dimension = item.Dimension;
                        facs.Add(room);
                        break;
                    default:
                        Console.WriteLine("Not found"); 
                        break;
                }
                //Debug.WriteLine(item.AreaType);
                //if (item.AreaType.Equals("Cinema"))
                //{
                //    Cinema cinema = new Cinema();
                //    cinema.Dimension = item.Dimension;
                //    cinema.Position = item.Position;

                //}
            }
            Settings settings = JsonConvert.DeserializeObject<Settings>(text);
            //Console.WriteLine(settings.CinemaTimeUnit + " " + settings.RestaurantTimeUnit + " " + settings.StairsTimeUnit + " " + settings.DeathTimeUnit + " " + settings.CleaningTimeUnit + " " + settings.CleaningEmergengyTimeUnit);
            return settings;
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
            StreamWriter sw = new StreamWriter(@"../../Settings.json");
            jsonSerializer.Serialize(sw, settings);
            sw.Close();
        }
    }
}

// 28 SEPT
