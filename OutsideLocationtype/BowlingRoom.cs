using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HotelSimulatie.Actors;
using HotelSimulatie.Facilities;
using System.Drawing;
using HotelSimulatie.Factory;


namespace OutsideLocationtype
{
    public class BowlingRoom : LocationType
    {
        public BowlingRoom(Facility specs) : base(specs)
        {
            Image = Image.FromFile("../../Resources/restaurant.png");
            IFactory locationTypeFactory = AbstractFactory.Instance().Create("LocationType");
            (locationTypeFactory as ConcreteCreatableObjectFactory).RegisterType<BowlingRoom>("BowlingRoom");
        }

        public override void Interact(Human human)
        {
            JoinFacility(human);
        }

        public override void Update(double drawUpdateTime)
        {

        }
    }
}
