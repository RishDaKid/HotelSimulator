using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using HotelSimulatie.Model;
using System.IO;

namespace HotelSimulatie.GUI
{
    public partial class SettingsScreen : UserControl
    {
        JsonSerializer jsonSerializer;
        Settings settings;
        public SettingsScreen()
        {
            InitializeComponent();
            jsonSerializer = new JsonSerializer();
            settings = new Settings();
        }

        private void buConfirm_Click(object sender, EventArgs e)
        {
            WriteFile();
           // ReadFile();
        }

        public Settings ReadFile()
        {
            string text = File.ReadAllText(@"../../settings.json");
            settings = JsonConvert.DeserializeObject<Settings>(text);
            //Console.WriteLine(settings.CinemaTimeUnit + " " + settings.RestaurantTimeUnit + " " + settings.StairsTimeUnit + " " + settings.DeathTimeUnit + " " + settings.CleaningTimeUnit + " " + settings.CleaningEmergengyTimeUnit);
            return settings;
        }

        private void WriteFile()
        {
            settings.CinemaTimeUnit = Convert.ToInt32(tbCinema.Text) * Convert.ToInt32(comboBox1.SelectedText);
            settings.RestaurantTimeUnit = Convert.ToInt32(tbRestaurant.Text) * Convert.ToInt32(comboBox1.SelectedText);
            settings.StairsTimeUnit = Convert.ToInt32(tbStairs.Text) * Convert.ToInt32(comboBox1.SelectedText);
            settings.DeathTimeUnit = Convert.ToInt32(tbDead.Text) * Convert.ToInt32(comboBox1.SelectedText);
            settings.CleaningTimeUnit = Convert.ToInt32(tbCleaning.Text) * Convert.ToInt32(comboBox1.SelectedText);
            settings.CleaningEmergengyTimeUnit = Convert.ToInt32(tbCleaningEmergency.Text) * Convert.ToInt32(comboBox1.SelectedText);


            StreamWriter sw = new StreamWriter(@"../../settings.json");
            jsonSerializer.Serialize(sw, settings);
            sw.Close();
        }

        private void buConfirm_Click_1(object sender, EventArgs e)
        {
            this.Hide();
        }
    }
}
