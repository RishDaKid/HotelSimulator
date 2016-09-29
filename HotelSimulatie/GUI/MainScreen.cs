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
        FileReader fileReader;

        public Form1()
        {
            InitializeComponent();
            fileReader = new FileReader();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            //fileReader.ReadLayout();

            SettingsScreen SettingsScreen = new SettingsScreen();
            Controls.Add(SettingsScreen);

            Room room2a = new Room() { Naam = "room2a" };
            Node hallwayRoom2b = new Hallway() { Naam = "hallwayRoom2b" };
            Node hallwayRoom2a = new Hallway() { Naam = "hallwayRoom2a" };

            // remove
            //Node hallwayRoom2c = new Hallway() { Naam = "hallwayRoom2b" };

            room2a.Neighbors.Add(hallwayRoom2a, 1); //1 voor in en uit stappen, 1 voor naar de gang te lopen.
            hallwayRoom2a.Neighbors.Add(room2a, 1); //1 voor in en uit stappen, 1 voor naar de gang te lopen.

            //room2a.Neighbors.Add(hallwayRoom2c, 5);


            hallwayRoom2a.Neighbors.Add(hallwayRoom2b, 1);
            //hallwayRoom2b.Neighbors.Add(hallwayRoom2a, 1);

            Visitor visit = new Visitor();
            //visit.room = room2a;
            visit.location = room2a;
            label1.Text = visit.CreatePath(hallwayRoom2b); // stuur destinatie mee. 

            Console.ReadLine();
        }

        private void bSettings_Click(object sender, EventArgs e)
        {
            SettingsScreen.BringToFront();
            SettingsScreen.Visible = true;
        }
    }
}
