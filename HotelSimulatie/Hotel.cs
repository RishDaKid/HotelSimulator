using HotelSimulatie.Actors;
using HotelSimulatie.Facilities;
using HotelSimulatie.Graph;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelSimulatie
{

    class Hotel
    {
        public List<Node> Facilities { get; set; }
        public List<Node> Rooms { get; set; }

        private FileReader fileReader;
        private Bitmap WorldBitmap;
        private Form1 Form;


        public Hotel(Form1 _form)
        {
            fileReader = new FileReader(); 
            WorldBitmap = new Bitmap(1300, 700);
            Facilities = new List<Node>();
            Rooms = new List<Node>();

            AssembleHotel();

            this.Form = _form;
            Form.Paint += Form_Paint;
        }

        private void Form_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            e.Graphics.DrawImage(WorldBitmap, new Point(7, 7));
        }


        public void AssembleHotel()
        {
            #region Create objects from the list of model facilities
            List<Facility> facilitiesModels = fileReader.ReadLayoutFile();

            // Haal alle faciliteiten uit de model list van faciliteiten 
            foreach (var item in facilitiesModels)
            {
                if (item.AreaType.Equals("Cinema"))
                {
                    Cinema cinema = new Cinema();
                    cinema.Position = item.Position;
                    cinema.Dimension = item.Dimension;
                    Facilities.Add(cinema);
                }
                else if (item.AreaType.Equals("Restaurant"))
                {
                    Restaurant restaurant = new Restaurant();
                    restaurant.Capacity = item.Capacity;
                    restaurant.Position = item.Position;
                    restaurant.Dimension = item.Dimension;
                    Facilities.Add(restaurant);
                }
                else if (item.AreaType.Equals("Room"))
                {
                    Room room = new Room();
                    room.Position = item.Position;
                    room.Dimension = item.Dimension;
                    room.Classification = item.Classification;
                    Rooms.Add(room);
                }
                else
                {
                    Fitnesscentrum fitnessCentrum = new Fitnesscentrum();
                    fitnessCentrum.Position = item.Position;
                    fitnessCentrum.Dimension = item.Dimension;
                    Facilities.Add(fitnessCentrum);
                }
            }
            #endregion

            // ######################
            // Hier handmatig faciliteiten toevoegen
            // ######################

            // 7, 5 is het eerste vakje rechtsonder

            Lobby lobby = new Lobby();
            Point point = new Point(7, 4);
            lobby.Position = point;

            #region commented
            //Node hallwayRoom2b = new Hallway() { Naam = "hallwayRoom2b" };
            //Node hallwayRoom2a = new Hallway() { Naam = "hallwayRoom2a" };

            //// remove
            //Node hallwayRoom2c = new Hallway() { Naam = "hallwayRoom2b" };

            //room2a.Neighbors.Add(hallwayRoom2a, 10); //1 voor in en uit stappen, 1 voor naar de gang te lopen.

            //room2a.Neighbors.Add(hallwayRoom2c, 5);


            //hallwayRoom2a.Neighbors.Add(hallwayRoom2b, 20);
            //hallwayRoom2b.Neighbors.Add(hallwayRoom2a, 1);

            //Visitor visit = new Visitor();
            ////visit.room = room2a;
            //visit.location = room2a;
            //Console.WriteLine(visit.CreatePath(hallwayRoom2b));// stuur destinatie mee.
            #endregion

            #region Get all facilities and rooms and put them in one list (Draw methods need these)

            // Voeg de plekken die je handmatig hebt aangemaakt toe aan de lijst
            Facilities.Add(lobby);
            DrawTiles(Facilities);
            DrawTiles(Rooms);
            #endregion

        }

        public void SeparateFacilities()
        {

        }

        Node[,] tileArray;
        private Form1 form1;
        int maxX;
        int maxY;

        public void AssignPiecesToTiles(List<Node> facilities)
        {
            //maxX = facilities.Max(x => x.Position.X);
            //maxY = facilities.Max(x => x.Position.Y);

            //tileArray = new Node[maxX + 1, maxY + 1];

            //foreach (var item in facilities)
            //{
            //    tileArray[item.Position.X, item.Position.Y] = item;
            //}

            //// testing purpose
            //for (int x = 0; x < maxX; x++)
            //{

            //    for (int y = 0; y < maxY; y++)
            //    {
            //        if (tileArray[x, y] != null)
            //        {
            //            Console.WriteLine(tileArray[x, y].Naam);
            //        }
            //        else
            //        {
            //            tileArray[x, y] = new Empty();
            //            Console.WriteLine("Empty");
            //        }
            //    }
            //}
        } 

        private void DrawTiles(List<Node> places)
        {
            foreach (var item in places)
            {
                using (Graphics Canvas = Graphics.FromImage(WorldBitmap))
                {
                    Canvas.DrawImage(item.TileImage, item.Position.X * 130, item.Position.Y * 50, 130, 50);
                }
            }
            //for (int x = 0; x < maxX; x++)
            //{
            //    for (int y = 0; y < maxY; y++)
            //    {
            //        using (Graphics Canvas = Graphics.FromImage(WorldBitmap))
            //        {
            //            Canvas.DrawImage(tileArray[x, y].TileImage, x * 130, y * 50, 130, 50);
            //        }
            //    }
            //}

            //for (int x = 0; x < maxX; x++)
            //{
            //    for (int y = 0; y < maxY; y++)
            //    {
            //        using (Graphics Canvas = Graphics.FromImage(WorldBitmap))
            //        {
            //            Canvas.DrawImage(tileArray[x, y].TileImage, x * 130, y * 50, 130, 50);
            //        }
            //    }
            //}

        }

    }
}
