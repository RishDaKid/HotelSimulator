using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HotelSimulatie.Actors;
using HotelSimulatie.Facilities;
using HotelSimulatie.Graph;
using HotelSimulatie.GUI;

namespace HotelSimulatie
{
    public partial class Form1 : Form
    {
        SettingsScreen SettingsScreen;

        public Form1()
        {
            SettingsScreen = new SettingsScreen();
            Controls.Add(SettingsScreen);
            SettingsScreen.BringToFront();
            SettingsScreen.Visible = false;
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Hotel hotel = new Hotel(this);



            Console.ReadLine();
        }

        private void bSettings_Click(object sender, EventArgs e)
        {
            SettingsScreen.Visible = true;
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
