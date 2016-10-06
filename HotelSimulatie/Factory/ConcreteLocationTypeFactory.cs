using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HotelSimulatie.Facilities;

namespace HotelSimulatie.Factory
{
    class ConcreteLocationTypeFactory : LocationTypeFactory
    {
        public override LocationType Create(string locationType)
        {
            switch (locationType)
            {
                case "Staircase":
                    return new Staircase();
                case "Lobby":
                    return new Lobby();
                case "Fitness":
                    return new Fitnesscentrum();
                case "Elevator":
                    return new Elevator();
                case "ElevatorHall":
                    return new ElevatorHall();
                case "Room":
                    return new Room();
                case "Restaurant":
                    return new Restaurant();
                case "Cinema":
                    return new Cinema();
                default: return null;
            }
        }
    }
}
