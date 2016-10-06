using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HotelSimulatie.GUI
{
    public partial class ObjectInformationPaper : UserControl
    {


        public ObjectInformationPaper()
        {
            InitializeComponent();
            pictureBox1.MouseClick += PictureBox1_MouseClick;
        }

        private void PictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            this.Visible = false;
        }
    }
}
