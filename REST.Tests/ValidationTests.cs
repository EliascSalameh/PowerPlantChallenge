using System;
using System.Collections.Generic;
using REST.Engine;
using REST.Engine.Entities;
using REST.Engine.Entities.ExtendedEntities;
using Xunit;

namespace REST.Tests
{
    public class ValidationTests
    {
        [Fact]
        public void ValidatePayload_CheckLoadIfEqualZero_ShouldThrowException()
        {
            //ARRANGE
            var payLoad = new PayLoad
            {
                Fuels = new Fuels {Co2Consumption = 0, GasPrice = 23, KerosinePrice = 25, WindPercentage = 0},
                Load = 0,
                PowerPlants = new List<PowerPlantExtended>()
            };
            //ASSERT
            Assert.Throws<Exception>(() => Validation.ValidatePayload(payLoad));
        }
    }
}