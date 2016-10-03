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
using HotelSimulatie.GUI;

namespace HotelSimulatie
{
    public partial class Form1 : Form
    {
        private SettingsScreen SettingsScreen;
        private ObjectInformationPaper paper;
        private Bitmap WorldBitmap;
        private Hotel hotel;

        public Form1()
        {
            InitializeComponent();
            hotel = new Hotel();

            SettingsScreen = new SettingsScreen();
            Controls.Add(SettingsScreen);
            SettingsScreen.BringToFront();
            SettingsScreen.Visible = false;

            paper = new ObjectInformationPaper();
            Controls.Add(paper);
           // paper.BringToFront();
            paper.Location = new Point(1480, 0);
            paper.Visible = false;

            WorldBitmap = new Bitmap(hotel._widthHotel, hotel._heightHotel);
            this.MouseClick += Form1_MouseClick;

            this.Paint += Form1_Paint;
            Console.ReadLine();
        }
        int begin;
        int Dimention;
        private void Form1_MouseClick(object sender, MouseEventArgs e)
        {
            begin = 0;
            Dimention = 160;
            begin = e.Location.X - hotel._hotelPosition.X;
            begin = (begin / Dimention);


            foreach (var itemm in hotel._facilities)
            {
                //Dimention = itemm.Dimension.X * 160;
                if (begin == itemm.Position.X)
                {
                    paper.label1.Text = itemm.AreaType;
                    paper.Visible = true;
                    break;
                }
            }
            label1.Text = begin.ToString();
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            foreach (var item in hotel._facilities)
            {
                using (Graphics Canvas = Graphics.FromImage(WorldBitmap))
                {
                    Canvas.DrawImage(item.Image, item.Position.X * 160, item.Position.Y * - 120, item.Dimension.X * 160, item.Dimension.Y * 120);
                }
            }
            e.Graphics.DrawImage(WorldBitmap, hotel._hotelPosition);
        }
        private void bSettings_Click(object sender, EventArgs e)
        {
            SettingsScreen.Visible = true;
        }
    }
}
