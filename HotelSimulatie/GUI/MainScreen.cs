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
    public partial class MainScreen : Form
    {
        private SettingsScreen SettingsScreen;
        private Bitmap WorldBitmap;
        private Hotel hotel;

        public MainScreen()
        {
            InitializeComponent();

            SettingsScreen = new SettingsScreen();
            Controls.Add(SettingsScreen);
            SettingsScreen.BringToFront();
            SettingsScreen.Visible = false;

            hotel = new Hotel();
            hotel.SortList();
            hotel.AssembleHotel();
            WorldBitmap = new Bitmap(1300, 450);
            //WorldBitmap = new Bitmap(1300, 500);
            

            PaintHotel(hotel.Facilities);
        }

        private void MainScreen_Load(object sender, EventArgs e)
        {
            // ...
        }

        /// <summary>
        /// Tekent alle plekken op het scherm.
        /// </summary>
        /// <param name="places">Een lijst met alle plekken in het hotel.</param>
        private void PaintHotel(List<Node> places)
        {
            int width = WorldBitmap.Size.Width - 130;
            int height = WorldBitmap.Size.Height - 50;
            foreach (var item in places)
            {
                using (Graphics Canvas = Graphics.FromImage(WorldBitmap))
                {
                    #region test
                    //if ((item.Dimension.X > 1) || (item.Dimension.Y > 1))
                    //{
                    //    // Plaats kamer deur
                    //    Canvas.DrawImage(item.TileImage, item.Position.X * 130, item.Position.Y * 50, 130, 50);

                    //    // Vergroot de kamer
                    //    for (int i = 0; i < item.Dimension.X; i++)
                    //    {
                    //        Canvas.DrawImage(item.TileImage, item.Position.X * 130, item.Position.Y * 50, 130, 50);
                    //    }
                    //}
                    //else
                    //{
                    //Canvas.DrawImage(item.TileImage, item.Position.X * 130, item.Position.Y * 50, 130, 50);
                    //}
                    #endregion

                    // Room at x.1 and y.1 will be placed at x.130 and y.150
                    // maak hier constant van
                    Canvas.DrawImage(item.TileImage, width - (item.Position.X * 130), height - (item.Position.Y * 50), 130, 50);
                    //Canvas.DrawImage(item.TileImage, 0 * 130, 0 * 50, 130, 50);
                    // position = 1300 - (1 * 130)

                }
            }
        }

        private void bSettings_Click(object sender, EventArgs e)
        {
            SettingsScreen.Visible = true;
        }

        private void MainScreen_Paint(object sender, PaintEventArgs e)
        {
            //e.Graphics.DrawImage(WorldBitmap, new Point(-200, -300));
            e.Graphics.DrawImage(WorldBitmap, new Point(0, 0));
            //e.Graphics.DrawImage(WorldBitmap, new Point(7, 7));
        }
    }
}
