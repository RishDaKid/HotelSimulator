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
        private Bitmap WorldBitmap;
        List<Node> hh;

        Node[,] tileArray;
        int maxX;
        int maxY;

        public Form1()
        {
            InitializeComponent();
            SettingsScreen = new SettingsScreen();
            Controls.Add(SettingsScreen);
            SettingsScreen.BringToFront();
            SettingsScreen.Visible = false;
            WorldBitmap = new Bitmap(1300, 700);

            AssembleHotel();
            DrawTiles();
            this.Paint += Form1_Paint1;

            Console.ReadLine();
        }

        private void Form1_Paint1(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawImage(WorldBitmap, new Point(7, 7));
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        //public void AssignPiecesToTiles(List<Node> facilities)
        //{
        //    // Gets Facilities with biggest X and Y Numbers.
        //    maxX = facilities.Max(x => x.Position.X);
        //    maxY = facilities.Max(x => x.Position.Y);

        //    //tileArray = new Node[maxX + 1, maxY + 1];

        //    //foreach (var item in facilities)
        //    //{
        //    //    tileArray[item.Position.X, item.Position.Y] = item;
        //    //}

        //    //// testing purpose
        //    //for (int x = 0; x < maxX; x++)
        //    //{
        //    //    for (int y = 0; y < maxY; y++)
        //    //    {
        //    //        if (tileArray[x, y] != null)
        //    //        {
        //    //            Console.WriteLine(tileArray[x, y].Naam);
        //    //        }
        //    //        else
        //    //        {
        //    //            tileArray[x, y] = new Empty();
        //    //            Console.WriteLine("Empty");
        //    //        }
        //    //    }
        //    //}
        //}


        private void DrawTiles()
        {
            foreach (var item in collection)
            {
                using (Graphics Canvas = Graphics.FromImage(WorldBitmap))
                {
                    Canvas.DrawImage(tileArray[x, y].TileImage, x * 130, y * 50, 130, 50);
                }
            }
        }

        public void AssembleHotel()
        {
            Hotel hotel = new Hotel();
            hh = hotel.creatHotel();
            //AssignPiecesToTiles();
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
