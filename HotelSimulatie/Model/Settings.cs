using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelSimulatie.Model
{
    public class Settings
    {
        //Where we will store the setting values
        public int Hte { get; set; }
        public double StairsTimeUnit { get; set; }
        public double RestaurantTimeUnit { get; set; }
        public double CinemaTimeUnit { get; set; }
        public double DeathTimeUnit { get; set; }
        public double CleaningTimeUnit { get; set; }
        public double CleaningEmergengyTimeUnit { get; set; }
        public double WalkingSpeedTimeUnit { get; set; }
        public double Queue { get; set; }
        public double Evacuation { get; set; }
        public double Godzilla { get; set; }
    }
}
