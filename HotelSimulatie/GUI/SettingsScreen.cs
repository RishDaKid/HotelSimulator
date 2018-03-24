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
        // values we use later on
        private FileReader fileReader;
        private FileWriter fileWriter;
        private char[] signs = { '@', '/', '!', '#', '$', '%', '^', '&', '*', '(', ')', '-', '_', '|', ';', '<', '>', '.', ',' };
        private int mimimumInputValue = 1;
        private int maximumInputValue = 4;

        /// <summary>
        /// Constructor
        /// </summary>
        public SettingsScreen()
        {
            InitializeComponent();
            fileReader = new FileReader();
            fileWriter = new FileWriter();

            // Setting those textboxes with the value of the settings file
            Settings settings = fileReader.GetSettings();
            comboBox1.Text = Convert.ToString(settings.Hte);
            tbGodzilla.Text = Convert.ToString(settings.Godzilla * Convert.ToDouble(comboBox1.Text));
            tbCinema.Text = Convert.ToString(settings.CinemaTimeUnit * Convert.ToDouble(comboBox1.Text));
            tbRestaurant.Text = Convert.ToString(settings.RestaurantTimeUnit * Convert.ToDouble(comboBox1.Text));
            tbStairs.Text = Convert.ToString(settings.StairsTimeUnit * Convert.ToDouble(comboBox1.Text));
            tbDead.Text = Convert.ToString(settings.DeathTimeUnit * Convert.ToDouble(comboBox1.Text));
            tbCleaning.Text = Convert.ToString(settings.CleaningTimeUnit * Convert.ToDouble(comboBox1.Text));
            tbWalkMovement.Text = Convert.ToString(settings.WalkingSpeedTimeUnit * Convert.ToDouble(comboBox1.Text)); 
            tbQueue.Text = Convert.ToString(settings.Queue * Convert.ToDouble(comboBox1.Text));
            tbEvacuation.Text = Convert.ToString(settings.Evacuation * Convert.ToDouble(comboBox1.Text));
            label9.Text = "";
        }

        /// <summary>
        /// Write input to the settings file
        /// </summary>
        private void SaveSettings()
        {
            Settings settings = new Settings();
            settings.Hte = Convert.ToInt32(comboBox1.Text);
            settings.Godzilla = Convert.ToDouble(tbGodzilla.Text) / Convert.ToDouble(comboBox1.Text);
            settings.StairsTimeUnit = Convert.ToDouble(tbStairs.Text) / Convert.ToDouble(comboBox1.Text);
            settings.RestaurantTimeUnit = Convert.ToDouble(tbRestaurant.Text) / Convert.ToDouble(comboBox1.Text);
            settings.CinemaTimeUnit = Convert.ToDouble(tbCinema.Text) / Convert.ToDouble(comboBox1.Text);
            settings.DeathTimeUnit = Convert.ToDouble(tbDead.Text) / Convert.ToDouble(comboBox1.Text);
            settings.CleaningTimeUnit = Convert.ToDouble(tbCleaning.Text) / Convert.ToDouble(comboBox1.Text);
            settings.WalkingSpeedTimeUnit = Convert.ToDouble(tbWalkMovement.Text) / Convert.ToDouble(comboBox1.Text);
            settings.Queue = Convert.ToDouble(tbQueue.Text) / Convert.ToDouble(comboBox1.Text);
            settings.Evacuation = Convert.ToDouble(tbEvacuation.Text) / Convert.ToDouble(comboBox1.Text);

            fileWriter.WriteFile(settings);
        }

        /// <summary>
        /// When we want to set the results of the given input
        /// The input should be an integer
        /// The input should be between the one and 9
        /// When it doesn't meet up to the requirements, it will give an error notification
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bConfirm_Click_1(object sender, EventArgs e)
        {
            if (tbGodzilla.Text.Any(char.IsDigit) &&
                tbStairs.Text.Any(char.IsDigit) && tbRestaurant.Text.Any(char.IsDigit) && 
                tbCinema.Text.Any(char.IsDigit) && tbDead.Text.Any(char.IsDigit) && 
                tbCleaning.Text.Any(char.IsDigit) && tbWalkMovement.Text.Any(char.IsDigit) && 
                tbQueue.Text.Any(char.IsDigit) && tbEvacuation.Text.Any(char.IsDigit) &&
                Convert.ToInt32(tbGodzilla.Text) >= mimimumInputValue && Convert.ToInt32(tbGodzilla.Text) <= maximumInputValue &&
                Convert.ToInt32(tbStairs.Text) >= mimimumInputValue && Convert.ToInt32(tbStairs.Text) <= maximumInputValue &&
                Convert.ToInt32(tbRestaurant.Text) >= mimimumInputValue && Convert.ToInt32(tbRestaurant.Text) <= maximumInputValue &&
                Convert.ToInt32(tbCinema.Text) >= mimimumInputValue && Convert.ToInt32(tbCinema.Text) <= maximumInputValue &&
                Convert.ToInt32(tbDead.Text) >= mimimumInputValue && Convert.ToInt32(tbDead.Text) <= maximumInputValue &&
                Convert.ToInt32(tbCleaning.Text) >= mimimumInputValue && Convert.ToInt32(tbCleaning.Text) <= maximumInputValue &&
                Convert.ToInt32(tbWalkMovement.Text) >= mimimumInputValue && Convert.ToInt32(tbWalkMovement.Text) <= maximumInputValue &&
                Convert.ToInt32(tbQueue.Text) >= mimimumInputValue && Convert.ToInt32(tbQueue.Text) <= maximumInputValue &&
                Convert.ToInt32(tbEvacuation.Text) >= mimimumInputValue && Convert.ToInt32(tbEvacuation.Text) <= maximumInputValue &&
                !tbGodzilla.Text.Contains(Convert.ToString(signs)) &&
                !tbStairs.Text.Contains(Convert.ToString(signs)) && !tbRestaurant.Text.Contains(Convert.ToString(signs)) &&
                !tbCinema.Text.Contains(Convert.ToString(signs)) && !tbDead.Text.Contains(Convert.ToString(signs)) &&
                !tbCleaning.Text.Contains(Convert.ToString(signs)) && !tbWalkMovement.Text.Contains(Convert.ToString(signs)) &&
                !tbQueue.Text.Contains(Convert.ToString(signs)) && !tbEvacuation.Text.Contains(Convert.ToString(signs)))
            {
                SaveSettings();
                Visible = false;
                label9.Text = "";
            }
            else
            {
                label9.Text = "Alleen getallen van 1 t/m 4 toegestaan!";
            }
        }

        /// <summary>
        /// We use this to cancel the settingsscreen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bCancel_Click(object sender, EventArgs e)
        {
            Visible = false;
        }

        private void bDefault_Click(object sender, EventArgs e)
        {
            comboBox1.Text = "1";
            tbGodzilla.Text = "3";
            tbCinema.Text = "3";
            tbRestaurant.Text = "3";
            tbStairs.Text = "3";
            tbDead.Text = "3";
            tbCleaning.Text = "3";
            tbWalkMovement.Text = "3";
            tbQueue.Text = "3";
            tbEvacuation.Text = "3";
        }
    }
}
