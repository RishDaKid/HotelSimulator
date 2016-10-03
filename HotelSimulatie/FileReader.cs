using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using HotelSimulatie.Model;
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

        public List<Facility> ReadLayoutFile()
        {
            string text = File.ReadAllText(@"../../Hotel2.layout");
            List<Facility> Facilities = JsonConvert.DeserializeObject<List<Facility>>(text);

            return Facilities;
        }

        //public Settings GetSettings()
        //{
        //    string text = File.ReadAllText(@"../../settings.json");
        //    Settings settings = JsonConvert.DeserializeObject<Settings>(text);
        //    //Console.WriteLine(settings.CinemaTimeUnit + " " + settings.RestaurantTimeUnit + " " + settings.StairsTimeUnit + " " + settings.DeathTimeUnit + " " + settings.CleaningTimeUnit + " " + settings.CleaningEmergengyTimeUnit);
        //    return settings;
        //}

        //public void WriteFile(Settings settings)
        //{
        //    StreamWriter sw = new StreamWriter(@"../../settings.json");
        //    jsonSerializer.Serialize(sw, settings);
        //    sw.Close();
        //}
    }
}

// 28 SEPT
// http://stackoverflow.com/questions/15368231/can-json-numbers-be-quoted