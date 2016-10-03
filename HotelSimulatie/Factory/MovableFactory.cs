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
        private Dictionary<string, LocationTypeFactory> _factories;

        public MovableFactory()
        {
            _factories = new Dictionary<string, LocationTypeFactory>();
        }

        /// <summary>
        /// Create a new instance of <see cref="IMovableThing"/> depending on the given type
        /// </summary>
        /// <param name="factoryType">The type of factory to use</param>
        /// <param name="type">The type of <see cref="IMovableThing"/> to create</param>
        /// <returns>A new instance of <see cref="IMovableThing"/>, or null if the parameters are incorrect</returns>
        public LocationType Create(string factoryType, string type)
        {
            if (_factories.ContainsKey(factoryType))
                return _factories[factoryType].Create(type);

            return null;
        }

        public bool RegisterFactory(string type, LocationTypeFactory newFactory)
        {
            if (_factories.ContainsKey(type))
                return false;

            _factories.Add(type, newFactory);
            return true;
        }
    }
}
