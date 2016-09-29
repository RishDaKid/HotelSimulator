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
        private FileReader fileReader;
        private Bitmap WorldBitmap;
        private Form1 Form;

        public Hotel(Form1 _form)
        {
            fileReader = new FileReader(); 
            WorldBitmap = new Bitmap(1300, 700);
            AssembleHotel();
            DrawTiles();

            this.Form = _form;
            Form.Paint += Form_Paint;
        }

        private void Form_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            e.Graphics.DrawImage(WorldBitmap, new Point(7, 7));
        }

        public void AssembleHotel()
        {
            fileReader.ReadLayoutFile();

            Lobby lobby = new Lobby();

            //Room room2a = new Room() { Naam = "room2a" };
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

            #region Get all facilities and rooms and put them in one list
            List<Node> allFacilities = fileReader.FacilitiesFromFile;
            List<Node> rooms = fileReader.RoomsFromFile;
            foreach (var item in rooms)
            {
                allFacilities.Add(item);
            }

            // Voeg de plekken die je handmatig hebt aangemaakt toe aan de lijst
            allFacilities.Add(lobby);

            AssignPiecesToTiles(allFacilities);
            #endregion

        }

        Node[,] tileArray;
        private Form1 form1;
        int maxX;
        int maxY;

        public void AssignPiecesToTiles(List<Node> facilities)
        {
            maxX = facilities.Max(x => x.Position.X);
            maxY = facilities.Max(x => x.Position.Y);

            tileArray = new Node[maxX + 1, maxY + 1];

            foreach (var item in facilities)
            {
                tileArray[item.Position.X, item.Position.Y] = item;
            }

            // testing purpose
            for (int x = 0; x < maxX; x++)
            {

                for (int y = 0; y < maxY; y++)
                {
                    if (tileArray[x, y] != null)
                    {
                        Console.WriteLine(tileArray[x, y].Naam);
                    }
                    else
                    {
                        tileArray[x, y] = new Empty();
                        Console.WriteLine("Empty");
                    }
                }
            }
        } 

        private void DrawTiles()
        {
            for (int x = 0; x < maxX; x++)
            {
                for (int y = 0; y < maxY; y++)
                {
                    using (Graphics Canvas = Graphics.FromImage(WorldBitmap))
                    {
                        Canvas.DrawImage(tileArray[x, y].TileImage, x * 130, y * 50, 130, 50);
                    }
                }
            }

            //for (int x = 0; x < maxX; x++)
            //{
            //    for (int y = maxY; y > 0; y--)
            //    {
            //        using (Graphics Canvas = Graphics.FromImage(WorldBitmap))
            //        {
            //            Canvas.DrawImage(tileArray[x, y].TileImage, x * 130, y * 50, 130, 50);
            //        }
            //    }
            //}

            // for (int x = maxX; x > 0; x--)
            // {
            //     for (int y = maxY; y > 0; y--)
            //     {
            //         using (Graphics Canvas = Graphics.FromImage(WorldBitmap))
            //         {
            //             if (tileArray[x, y] != null)
            //             {
            //                 Canvas.DrawImage(tileArray[x, y].TileImage, x * 130, y * 50, 130, 50);
            //             }
            //         }
            //    } 
            //} 
        }

    }
}
