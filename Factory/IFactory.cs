using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Factory
{
    interface IFactory
    {
        LocationType CreateAreaType(string type, Facility specs);
        MovableCreatures CreateMovableCreatures(string type, IMovableParam humanParam);
    }
}
