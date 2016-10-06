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

            foreach (LocationType item in hotel._facilities)
            {

                if (item.Position.X == 9 && item.Position.Y == 1)
                {
                    listBox1.Items.Add(item.neighBor.Count());
                    listBox1.Items.Add(item.AreaType);
                }
            }
            Console.ReadLine();
        }
        Rectangle rec;

        private void Form1_MouseClick(object sender, MouseEventArgs e)
        {
            Dictionary<LocationType, Rectangle> location = new Dictionary<LocationType, Rectangle>();
            Point point = new Point(e.Location.X - hotel._hotelPosition.X, hotel._heightHotel - e.Location.Y + hotel._hotelPosition.Y);

            foreach (var item in hotel._facilities)
            {
                rec = new Rectangle(item.Position.X * item.Width, item.Position.Y * item.Height, item.Dimension.X * item.Width, item.Dimension.Y * item.Height);
                location.Add(item, rec);
            }

            foreach (var item in location)
            {
                if (point.X >= item.Value.X && point.X <= (item.Value.X + item.Value.Width) && point.Y >= item.Value.Y && point.Y <= (item.Value.Y + item.Value.Height))
                {
                    if (item.Key.AreaType.Equals("Cinema") || item.Key.AreaType.Equals("Fitness"))
                    {
                        paper.label1.Text = item.Key.AreaType;
                        break;
                    }
                    else if (item.Key.AreaType.Equals("Room"))
                    {
                        paper.label1.Text = item.Key.AreaType;
                        break;
                    }
                    else if (item.Key.AreaType.Equals("Restaurant"))
                    {
                        paper.label1.Text = item.Key.AreaType;
                        paper.label5.Text = ((item.Key) as Restaurant).Capacity.ToString();
                        break;
                    }
                    else if (item.Key.AreaType.Equals("Lobby"))
                    {
                        paper.label1.Text = item.Key.AreaType;
                        paper.label5.Text = ((item.Key) as Lobby).Capacity.ToString();
                        break;
                    }
                    else if (item.Key.AreaType.Equals("ElevatorHall"))
                    {
                        paper.label1.Text = item.Key.AreaType;
                        break;
                    }
                    else if (item.Key.AreaType.Equals("Elevator"))
                    {
                        paper.label1.Text = item.Key.AreaType;
                        break;
                    }
                    else if (item.Key.AreaType.Equals("Staircase"))
                    {
                        paper.label1.Text = item.Key.AreaType;
                        break;
                    }
                    else
                    {
                        paper.label1.Text = item.Key.AreaType;
                        break;
                    }
                }
            }
            paper.Visible = true;
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            foreach (var item in hotel._facilities)
            {
                using (Graphics Canvas = Graphics.FromImage(WorldBitmap))
                {
                    Canvas.DrawImage(item.Image, item.Position.X * item.Width, WorldBitmap.Height - (item.Position.Y * item.Height) - item.Dimension.Y * item.Height, item.Dimension.X * item.Width, item.Dimension.Y * item.Height);
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
