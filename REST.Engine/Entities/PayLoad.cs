using System.Collections.Generic;
using REST.Engine.Entities.ExtendedEntities;

namespace REST.Engine.Entities
{
    public class PayLoad
    {
        public double Load { get; set; }
        public Fuels Fuels { get; set; }
        public List<PowerPlantExtended> PowerPlants { get; set; }
    }
}