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

        Node[,] tileArray;
        int maxX;
        int maxY;
        Hotel hotel;
        public Form1()
        {
            InitializeComponent();
            SettingsScreen = new SettingsScreen();
            Controls.Add(SettingsScreen);
            SettingsScreen.BringToFront();
            SettingsScreen.Visible = false;
            WorldBitmap = new Bitmap(1300, 700);
            hotel = new Hotel();


            DrawTiles(hotel.AssembleHotel());
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

        private void DrawTiles(List<Node> places)
        {
            foreach (var item in places)
            {
                using (Graphics Canvas = Graphics.FromImage(WorldBitmap))
                {
                    Canvas.DrawImage(item.TileImage, item.Position.X * 130, item.Position.Y * 50, 130, 50);
                }
            }
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
