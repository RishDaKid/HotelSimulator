using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HotelSimulatie.Facilities;

namespace HotelSimulatie.Factory
{
    abstract class LocationTypeFactory
    {
        public abstract LocationType Create(string locationType);

    }
}
