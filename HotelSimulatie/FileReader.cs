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

namespace HotelSimulatie
{
    class FileReader
    {
        JsonSerializer jsonSerializer;

        public FileReader()
        {
            jsonSerializer = new JsonSerializer();
        }

        public Settings JsonToObject() 
        {
            string text = File.ReadAllText(@"../../Hotel.layout");
            List<Node> ReadMovie = JsonConvert.DeserializeObject<List<Node>>(text);
            foreach (var item in ReadMovie)
            {
                Debug.WriteLine(item);
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
