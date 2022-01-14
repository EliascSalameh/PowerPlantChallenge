using System.IO;
using System.Reflection;
using System.Text.Json;
using AutoFixture.Xunit2;
using REST.Engine;
using REST.Engine.Entities;
using REST.Engine.Enum;
using REST.Tests.Data;
using Xunit;

namespace REST.Tests
{
    public class CalculationTests
    {
        [Theory]
        [InlineAutoData(@"JSONFiles\payload1.json")]
        public void GetActualCostAndPMax_CheckActualCostValue_ShouldBeDifferentThanZero(string payLoadFile)
        {
            //ARRANGE
            var content = File.ReadAllText(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), payLoadFile));
            var payLoad = JsonSerializer.Deserialize<PayLoad>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
            //ACT
            var result = Calculation.GetActualCostAndPMax(payLoad);
            //ASSERT
            foreach (var item in result)
                if (item.Type != PowerPlantTypeEnum.WindTurbine)
                    Assert.NotEqual(0, item.ActualFuelCostPerMWh);
        }

        [Theory]
        [ClassData(typeof(PayLoadData))]
        public void GetActualCostAndPMaxMultipleInput_CheckActualPMaxValue_ShouldBeDifferentThanZero(PayLoad payLoad)
        {
            //ACT
            var result = Calculation.GetActualCostAndPMax(payLoad);
            //ASSERT
            foreach (var item in result)
                Assert.NotEqual(0, item.ActualPMax);
        }
    }
}