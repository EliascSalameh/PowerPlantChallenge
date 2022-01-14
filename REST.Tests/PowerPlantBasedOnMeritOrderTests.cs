using System.IO;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;
using AutoFixture.Xunit2;
using REST.Engine;
using REST.Engine.Entities;
using Xunit;

namespace REST.Tests
{
    public class PowerPlantBasedOnMeritOrderTests
    {
        [Theory]
        [InlineAutoData(@"JSONFiles\payload11.json")]
        public async Task ExecuteProcess_CheckProductionResultsListIfAnyCombinationFound_ShouldReturnTrue(string payLoadFile, PowerPlantBasedOnMeritOrder sut)
        {
            //Arrange
            var content = File.ReadAllText(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), payLoadFile));
            var payLoad = JsonSerializer.Deserialize<PayLoad>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
            //ACT
            var combination = await sut.ExecuteProcess(payLoad);
            //Assert
            Assert.NotEmpty(combination.ProductionResults);
        }
    }
}