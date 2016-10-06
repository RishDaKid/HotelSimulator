using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
//using Newtonsoft.Json;
using HotelSimulatie.Model;
using System.IO;
using HotelSimulatie.FileHandeling;

namespace HotelSimulatie.GUI
{
    public partial class SettingsScreen : UserControl
    {
        FileReader fileReader;
        FileWriter fileWriter;
        public SettingsScreen()
        {
            InitializeComponent();
            fileReader = new FileReader();
            fileWriter = new FileWriter();

            // Scheelt werk bij testen
            comboBox1.Text = "1";
            tbCinema.Text = "1";
            tbRestaurant.Text = "2";
            tbStairs.Text = "3";
            tbDead.Text = "4";
            tbCleaning.Text = "5";
            tbCleaningEmergency.Text = "6";
        }

        private void buConfirm_Click(object sender, EventArgs e)
        {
            SaveSettings();
        }

        /// <summary>
        /// Schrijft de ingevulde gegevens naar een settings bestand.
        /// </summary>
        private void SaveSettings()
        {
            Settings settings = new Settings();
            settings.StairsTimeUnit = Convert.ToInt32(tbStairs.Text) * Convert.ToInt32(comboBox1.Text);
            settings.RestaurantTimeUnit = Convert.ToInt32(tbRestaurant.Text) * Convert.ToInt32(comboBox1.Text);
            settings.CinemaTimeUnit = Convert.ToInt32(tbCinema.Text) * Convert.ToInt32(comboBox1.Text);
            settings.DeathTimeUnit = Convert.ToInt32(tbDead.Text) * Convert.ToInt32(comboBox1.Text);
            settings.CleaningTimeUnit = Convert.ToInt32(tbCleaning.Text) * Convert.ToInt32(comboBox1.Text);
            settings.CleaningEmergengyTimeUnit = Convert.ToInt32(tbCleaningEmergency.Text) * Convert.ToInt32(comboBox1.Text);

            fileWriter.WriteFile(settings);
        }

        private void bConfirm_Click_1(object sender, EventArgs e)
        {
            SaveSettings();
            Visible = false;
        }

        private void bCancel_Click(object sender, EventArgs e)
        {
            Visible = false;
        }

    }
}
