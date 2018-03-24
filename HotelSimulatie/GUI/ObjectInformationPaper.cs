using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HotelSimulatie.Facilities;

namespace HotelSimulatie.GUI
{
    public partial class ObjectInformationPaper : UserControl
    {

        /// <summary>
        /// consturctor
        /// </summary>
        public ObjectInformationPaper()
        {
            InitializeComponent();
            pictureBox1.MouseClick += PictureBox1_MouseClick;
        }

        /// <summary>
        /// Clickable and make the visibility false
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            this.Visible = false;
        }

        /// <summary>
        /// Setting the info off the papers
        /// </summary>
        /// <param name="item"></param>
        public void SetPaperInfo(LocationType item)
        {
            lbClassification.Text = "";
            // Set areatype label
            lbAreaType.Text = item.AreaType;
            // This is for the amout of people in the areatype
            lbQuantity.Text = item.CurrentHumans.Count().ToString();

            lbID.Text = "";

            foreach (var human in item.CurrentHumans)
            {
                lbID.Text += human.ID.ToString() + " ";
            }

            if(item is Room)
            {
                lbClassification.Text = (item as Room).Classification.ToString();
            }
        }
    }
}
