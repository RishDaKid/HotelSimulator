using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HotelSimulatie.Facilities;

namespace HotelSimulatie.Factory
{
    class MovableFactory
    {
        /// <summary>All the factories registered to this abstract factory. Key = factoryType, Value = the factory itself</summary>
        private Dictionary<string, LocationTypeFactory> factories;

        public MovableFactory()
        {
            factories = new Dictionary<string, LocationTypeFactory>();
        }

        /// <summary>
        /// Create a new instance of <see cref="IMovableThing"/> depending on the given type
        /// </summary>
        /// <param name="factoryType">The type of factory to use</param>
        /// <param name="type">The type of <see cref="IMovableThing"/> to create</param>
        /// <returns>A new instance of <see cref="IMovableThing"/>, or null if the parameters are incorrect</returns>
        public LocationType Create(Facility layout)
        {
            if (factories.ContainsKey(layout.AreaType))
                return factories[layout.AreaType].Create(layout);

            return null;
        }

        public bool RegisterFactory(string type, LocationTypeFactory newFactory)
        {
            if (factories.ContainsKey(type))
                return false;

            factories.Add(type, newFactory);
            return true;
        }
    }
}
