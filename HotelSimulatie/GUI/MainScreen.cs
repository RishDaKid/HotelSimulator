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
using HotelEvents;
using System.Diagnostics;
using HotelSimulatie.Monster;
using HotelSimulatie.Factory;
using HotelSimulatie.Model;

namespace HotelSimulatie
{
    public partial class Form1 : Form
    {
        // When we click on a areatype we will see the information in it on this paper
        private ObjectInformationPaper paper;
        // This is the usercontrol we show at the beginning of the programm
        private StartScreen startScreen;
        // We create a hotel object in the world (mainscreen)
        private Hotel hotel;
        // Tiemr to update this game. Every class has an update which is activated by this timer
        private Timer updateTimer;
        // To get the amount of time it takes to update and draw once
        private Stopwatch watch;
        // WorldBitmap is the bitmap we are going to draw on
        private Bitmap worldBitmap;
        // Creating the godzilla creature here because this is the world
        private Godzilla godzilla;
        // Size of the bitmap
        private Size bitmapSize = new Size();
        // This is how we set the location of the bitmap
        private Rectangle bitmapLocation;
        // Drag code
        private bool dragging = false;
        // Offset from mouse down position and image upper left corner
        private int offsetX;
        private int offsetY;       
        // We make a filereader instance to read the settings file
        private Settings hte;

        public Form1()
        {
            InitializeComponent();
            watch = new Stopwatch();
            startScreen = new StartScreen();
            startScreen.Location = new Point(340, 150);
            Controls.Add(startScreen);
            startScreen.BringToFront();
            startScreen.VisibleChanged += StartScreen_VisibleChanged;
        }

        /// <summary>
        /// When we start the game we initialize hotel, godzilla button and the paper
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StartScreen_VisibleChanged(object sender, EventArgs e)
        {
            // When the startscreen isn't visible
            if (startScreen.Visible == false)
            {
                // Godzilla activator button
                #region
                Button GodzillaButton = new Button();
                GodzillaButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 18.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                GodzillaButton.Location = new System.Drawing.Point(214, 132);
                GodzillaButton.Name = "button1";
                GodzillaButton.Size = new System.Drawing.Size(175, 78);
                GodzillaButton.TabIndex = 10;
                GodzillaButton.Text = "Godzilla";
                GodzillaButton.UseVisualStyleBackColor = true;
                this.Controls.Add(GodzillaButton);
                GodzillaButton.Click += GodzillaButton_Click;
                #endregion
                FileReader hteReader = new FileReader();
                // Read the settingsfile into HTE value
                hte = hteReader.GetSettings();

                // Create Hotel
                hotel = new Hotel(startScreen.ChosenFile, hte);
                bitmapSize.Height = this.Height;
                bitmapSize.Width = this.Width;

                // initialize the height and with from hotel
                BitmapInitialise();

                // Create the Clickable paper to see what's inside an Area-type
                paper = new ObjectInformationPaper();
                // Add the paper to the screen
                Controls.Add(paper);
                // Giving the paper a location
                paper.Location = new Point(1480, 0);
                // It shouldn't be visible yet
                paper.Visible = false;

                // For redrawing the bitmap
                this.Paint += Form1_Paint;

                // The gameloop starts here
                updateTimer = new Timer();
                // 17 to get a full 60 fps
                updateTimer.Interval = 17;
                updateTimer.Tick += UpdateTimer_Tick;
                updateTimer.Start();
            }
        }

        /// <summary>
        /// With a button click we create godzilla
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GodzillaButton_Click(object sender, EventArgs e)
        {
            CreateGodzilla();
        }

        /// <summary>
        /// Dynamically giving the Bitmap a size
        /// If hotel is smaller then the screen then give the screen size to the bitmap
        /// Or else, give the hotel size to the bitmap
        /// </summary>
        private void BitmapInitialise()
        {
            if (hotel.HotelWidth < this.Width)
            {
                bitmapSize.Width = this.Width;
            }
            else
            {
                LocationType biggestXpos = hotel.Facilities.Aggregate((i1, i2) => (i1.Position.X + i1.Dimension.X) > (i2.Position.X + i2.Dimension.X) ? i1 : i2);
                bitmapSize.Width = (biggestXpos.Position.X + biggestXpos.Dimension.X) * biggestXpos.oneDimensionSize.Width;
            }
            if (hotel.HotelHeight < this.Height)
            {
                bitmapSize.Height = this.Height;
            }
            else
            {
                LocationType biggestYpos = hotel.Facilities.Aggregate((i1, i2) => (i1.Position.Y + i1.Dimension.Y) > (i2.Position.Y + i2.Dimension.Y) ? i1 : i2);
                bitmapSize.Height = (biggestYpos.Position.Y + biggestYpos.Dimension.Y) * biggestYpos.oneDimensionSize.Height;
            }
            worldBitmap = new Bitmap(bitmapSize.Width, bitmapSize.Height);
            bitmapLocation.X = 0;
            bitmapLocation.Y = this.Height - bitmapSize.Height - 35;
        }

        /// <summary>
        /// We draw the hotel and all in it at every interval
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private double nextUpdate = 0;
        private void UpdateTimer_Tick(object sender, EventArgs e)
        {
            // We take the time of drawing and updating once
            double elapsedMs = watch.ElapsedMilliseconds;
            // Reset the time 
            watch.Reset();
            // Start it again
            watch.Start();
            // change
            hotel.Update(nextUpdate);
            // Create / Update godzilla
            if(godzilla != null)
            godzilla.Update(nextUpdate);
            // draw
            this.Invalidate();
            // Get number of the last tick update/draw time
            nextUpdate = elapsedMs;
            // This is so we can see the time
            count.Text = hotel.HotelTimer.ElapsedMilliseconds.ToString();
            // Reset the value to start the next count of draw/update
            elapsedMs = 0;
        }

        /// <summary>
        /// Godzilla is created in the world(mainscreen)
        /// </summary>
        public void CreateGodzilla()
        {
            if (godzilla == null)
            {
                // Create the factory to create cleaners
                IFactory locationTypeFactory = Factory.AbstractFactory.Instance().Create("MovableCreatures");
                LocationType biggest = hotel.Facilities.Aggregate((i1, i2) => i1.Position.Y > i2.Position.Y ? i1 : i2);

                // The information godzilla needs to set his values
                //MovableParam initialiseParam = new MovableParam() { height = this.Height / 100 * 80, width = this.Width / 100 * 50, hotel = hotel };
                MovableParam initialiseParam = new MovableParam() { Height = ((this.Height / 100) * 80), Width = ((this.Width / 100) *  50), Hotel = hotel, Settings = this.hte };

                // Create two cleaners based on the string
                godzilla = locationTypeFactory.CreateMovableCreatures("Godzilla", initialiseParam) as Godzilla;
            }
        }

        /// <summary>
        /// Painting the bitmap
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            // We draw the bitmap to the screen
            Graphics Canvas = Graphics.FromImage(worldBitmap);

            // Check if Hotel is not null to do this act
            if (hotel != null)
            {
                // Ask those items to draw them self
                hotel.DrawHotel(Canvas, worldBitmap);
            }
            // Check if Godzilla is not null to do this act
            if (godzilla != null)
            {
                // Ask those items to draw them self
                godzilla.Draw(Canvas, worldBitmap);
            }
            // Draw items into bitmap
            e.Graphics.DrawImage(worldBitmap, bitmapLocation.X, bitmapLocation.Y);

            // Resetting the background to transparatn
            Canvas.Clear(Color.Transparent);
        }



        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            // See if we're dragging.
            if (dragging)
            {
                // We're dragging the image. Move it.
                bitmapLocation.X = e.X + offsetX;
                bitmapLocation.Y = e.Y + offsetY;

                // Redraw.
                Invalidate();
            }
            else
            {
                // We're not dragging the image. See if we're over it.
                Cursor new_cursor = Cursors.Default;
                new_cursor = Cursors.Hand;

                if (Cursor != new_cursor)
                    Cursor = new_cursor;
            }
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            // Start dragging.
            dragging = true;

            // Save the offset from the mouse to the picture's origin.
            offsetX = bitmapLocation.X - e.X;
            offsetY = bitmapLocation.Y - e.Y;
        }

        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            dragging = false;
        }

        /// <summary>
        /// For clicking on the form to see a paper on the right-up side of the screen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Rectangle rec = new Rectangle();
            Dictionary<LocationType, Rectangle> location = new Dictionary<LocationType, Rectangle>();
            Point point = new Point(e.Location.X, this.Height - e.Location.Y - 35);

            foreach (var item in hotel.Facilities)
            {
                rec = new Rectangle(item.Position.X * item.oneDimensionSize.Width, item.Position.Y * item.oneDimensionSize.Height, item.Dimension.X * item.oneDimensionSize.Width, item.Dimension.Y * item.oneDimensionSize.Height);
                location.Add(item, rec);
            }

            foreach (var item in location)
            {
                if (point.X >= item.Value.X && point.X <= (item.Value.X + item.Value.Width) && point.Y >= item.Value.Y && point.Y <= (item.Value.Y + item.Value.Height))
                {
                    paper.SetPaperInfo(item.Key);
                }
            }
            paper.Visible = true;
        }
    }
}
