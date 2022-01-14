using System;
using REST.Engine.Entities;

namespace REST.Engine
{
    public static class Validation
    {
        public static void ValidatePayload(PayLoad payLoad)
        {
            if (payLoad.Load == 0)
                throw new Exception("Power Load set to 0");
            if (payLoad.Fuels == null)
                throw new Exception("Fuels not set");
            if (payLoad.PowerPlants == null)
                throw new Exception("PowerPlants not set");
        }
    }
}