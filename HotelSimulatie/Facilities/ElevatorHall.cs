using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace HotelSimulatie.Facilities
{
    public class ElevatorHall : LocationType
    {
        public ElevatorHall()
        {
            Image = Image.FromFile("../../Resources/ElevatorHall.png");
        }
    }
}

// class gemaakt. die noem je dijkstra node. Die krijgt bij het aanmaken een node. je maakt int afstand. dictionarry buren. buren = new dictionarry. foreach : key.value.