using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Factory
{
    public class Factories
    {
        private static Factories instance;
        public Dictionary<string, IFactory> factories;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <remarks>protected because of Singleton pattern</remarks>
        protected Factories()
        {
            factories = new Dictionary<string, IFactory>();
        }

        /// <summary>
        /// Creating objects
        /// </summary>
        /// <param name="factoryType"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public IFactory Create(string factoryType)
        {
            if (factories.ContainsKey(factoryType))
                return factories[factoryType];

            return null;
        }

        /// <summary>
        /// Register factory and it has to inherit from the interface "IFactory"
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="type"></param>
        /// <param name="newFactory"></param>
        /// <returns></returns>
        public bool RegisterFactory<T>(string type, T newFactory) where T : IFactory
        {
            if (factories.ContainsKey(type))
                return false;

            factories.Add(type, newFactory);
            return true;
        }

        /// <summary>
        /// do something
        /// </summary>
        /// <returns></returns>
        public static Factories Instance()
        {
            if (instance == null)
            {
                instance = new Factories();
            }
            return instance;
        }
    }
}
