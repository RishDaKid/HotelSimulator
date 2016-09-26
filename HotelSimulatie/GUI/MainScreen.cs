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
        public Form1()
        {
            InitializeComponent();


        }

        private void Form1_Load(object sender, EventArgs e)
        {

            SettingsScreen SettingsScreen = new SettingsScreen();
            Controls.Add(SettingsScreen);
            SettingsScreen.BringToFront();

            Visitor visitor1 = new Visitor();
            Visitor visitor2 = new Visitor();
            Visitor visitor3 = new Visitor();


            Graph.Graph graph = new Graph.Graph();

            Node lift2Node = new LiftNode() { Naam = "lift2" };
            Node room2a = new Room() { Naam = "room2a" };
            Node hallwayRoom2a = new Hallway() { Naam = "hallwayRoom2a" };
            Node room2b = new Room() { Naam = "room2c" };
            Node hallwayRoom2b = new Hallway() { Naam = "hallwayRoom2b" };
            Node room2c = new Room() { Naam = "room2c" };
            Node hallwayRoom2c = new Hallway() { Naam = "hallwayRoom2c" };
            Node stair2 = new Staircase() { Naam = "stair2" };

            Node lift1Node = new LiftNode() { Naam = "lift1" };
            Node restaurant1 = new Restaurant() { Naam = "restaurant1" };
            Node hallwayRestaurant1 = new Hallway() { Naam = "hallwayRestaurant1" };
            Node restaurant2 = new Restaurant() { Naam = "restaurant2" };
            Node hallwayRestaurant2 = new Hallway() { Naam = "hallwayRestaurant2" };
            Node stair1 = new Staircase() { Naam = "stair1" };

            Node lift0Node = new LiftNode() { Naam = "lift0" };
            Node fitnesscentrum = new Fitnesscentrum() { Naam = "fitnesscentrum" };
            Node hallwayFitnesscentrum = new Hallway() { Naam = "hallwayFitnesscentrum" };
            Node cinema = new Cinema() { Naam = "cinema" };
            Node hallwayCinema = new Hallway() { Naam = "hallwayCinema" };
            Node stair0 = new Staircase() { Naam = "stair0" };

            Node liftNode = new LiftNode() { Naam = "lift" };
            Node Lobby = new Lobby() { Naam = "Lobby" };
            Node hallwayLobby = new Hallway() { Naam = "hallwayLobby" };
            Node stair = new Staircase() { Naam = "stair" };

            room2c.Afstand = 0;

            lift2Node.Neighbors.Add(hallwayRoom2a, 2); //1 voor in en uit stappen, 1 voor naar de gang te lopen.
            lift2Node.Neighbors.Add(lift1Node, 1);

            lift1Node.Neighbors.Add(lift2Node, 1); // 1 voor verdieping
            lift1Node.Neighbors.Add(lift0Node, 1); // 1 voor verdieping
            lift1Node.Neighbors.Add(hallwayRestaurant1, 2); //1 voor in en uit stappen, 1 voor naar de gang te lopen.

            lift0Node.Neighbors.Add(lift1Node, 1); // 1 voor verdieping
            lift0Node.Neighbors.Add(liftNode, 1); // 1 voor verdieping
            lift0Node.Neighbors.Add(hallwayFitnesscentrum, 2); //1 voor in en uit stappen, 1 voor naar de gang te lopen.

            liftNode.Neighbors.Add(lift0Node, 1); // 1 voor verdieping
            liftNode.Neighbors.Add(hallwayLobby, 2); //1 voor in en uit stappen, 1 voor naar de gang te lopen.

            stair2.Neighbors.Add(hallwayRoom2c, 1);
            stair2.Neighbors.Add(stair1, 1);

            stair1.Neighbors.Add(hallwayRestaurant2, 1);
            stair1.Neighbors.Add(stair2, 1);
            stair1.Neighbors.Add(stair0, 1);

            stair0.Neighbors.Add(hallwayCinema, 1);
            stair0.Neighbors.Add(stair1, 1);
            stair0.Neighbors.Add(stair, 1);

            stair.Neighbors.Add(hallwayLobby, 1);
            stair.Neighbors.Add(stair0, 1);

            room2a.Neighbors.Add(hallwayRoom2a, 1); //1 voor in en uit stappen, 1 voor naar de gang te lopen.
            room2b.Neighbors.Add(hallwayRoom2b, 1); //1 voor in en uit stappen, 1 voor naar de gang te lopen.
            room2c.Neighbors.Add(hallwayRoom2c, 1); //1 voor in en uit stappen, 1 voor naar de gang te lopen.
            restaurant1.Neighbors.Add(hallwayRestaurant1, 1); //1 voor in en uit stappen, 1 voor naar de gang te lopen.
            restaurant2.Neighbors.Add(hallwayRestaurant2, 1); //1 voor in en uit stappen, 1 voor naar de gang te lopen.
            fitnesscentrum.Neighbors.Add(hallwayFitnesscentrum, 1); //1 voor in en uit stappen, 1 voor naar de gang te lopen.
            cinema.Neighbors.Add(hallwayCinema, 1); //1 voor in en uit stappen, 1 voor naar de gang te lopen.
            Lobby.Neighbors.Add(hallwayLobby, 1); //1 voor in en uit stappen, 1 voor naar de gang te lopen.

            hallwayLobby.Neighbors.Add(liftNode, 1);
            hallwayLobby.Neighbors.Add(stair, 1);
            hallwayLobby.Neighbors.Add(Lobby, 1);

            hallwayRoom2a.Neighbors.Add(lift2Node, 2);
            hallwayRoom2a.Neighbors.Add(hallwayRoom2b, 1);
            hallwayRoom2a.Neighbors.Add(room2a, 1);

            hallwayRoom2b.Neighbors.Add(hallwayRoom2a, 1);
            hallwayRoom2b.Neighbors.Add(hallwayRoom2c, 1);
            hallwayRoom2b.Neighbors.Add(room2b, 1);

            hallwayRoom2c.Neighbors.Add(hallwayRoom2b, 1);
            hallwayRoom2c.Neighbors.Add(stair2, 1);
            hallwayRoom2c.Neighbors.Add(room2c, 1);

            hallwayRestaurant1.Neighbors.Add(lift1Node, 2);
            hallwayRestaurant1.Neighbors.Add(hallwayRestaurant2, 1);
            hallwayRestaurant1.Neighbors.Add(restaurant1, 1);

            hallwayRestaurant2.Neighbors.Add(hallwayRestaurant1, 1);
            hallwayRestaurant2.Neighbors.Add(stair1, 1);
            hallwayRestaurant2.Neighbors.Add(restaurant2, 1);

            hallwayFitnesscentrum.Neighbors.Add(lift0Node, 2);
            hallwayFitnesscentrum.Neighbors.Add(hallwayCinema, 1);
            hallwayFitnesscentrum.Neighbors.Add(fitnesscentrum, 1);

            hallwayCinema.Neighbors.Add(stair0, 1);
            hallwayCinema.Neighbors.Add(hallwayFitnesscentrum, 1);
            hallwayCinema.Neighbors.Add(cinema, 1);

            string pad1 = graph.Dijkstra(room2c, Lobby);
            //label8.Text = pad1;
            Console.ReadLine();
        }
    }



}
