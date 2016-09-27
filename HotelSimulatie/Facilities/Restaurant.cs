﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HotelSimulatie.Graph;
using HotelSimulatie.Actors;

namespace HotelSimulatie.Facilities
{
    class Restaurant : Node
    {
        public int Capacity { get; set; }
        public string AreaType { get; set; }
        public int Position { get; set; }
        public int Dimention { get; set; }

        public Restaurant()
        {

        }
    }
}
