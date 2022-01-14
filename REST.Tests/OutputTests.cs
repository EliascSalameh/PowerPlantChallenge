using System;
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
    public class OutputTests
    {
        [Theory]
        [InlineAutoData(@"JSONFiles\payload1.json", 9313.15)]
        [InlineAutoData(@"JSONFiles\payload3.json", 20183.55)]
        [InlineAutoData(@"JSONFiles\payload4.json", 12134.4)]
        [InlineAutoData(@"JSONFiles\payload5.json", 144043.97)]
        [InlineAutoData(@"JSONFiles\payload6.json", 159791.66)]
        [InlineAutoData(@"JSONFiles\payload7.json", 10112)]
        [InlineAutoData(@"JSONFiles\payload8.json", 10112)]
        [InlineAutoData(@"JSONFiles\payload9.json", 22791.65)]
        [InlineAutoData(@"JSONFiles\payload11.json", 9313.15)]
        public async Task CalculateLoad_WhenCalled_ActualAndExpectedTotalCostShouldBeEqual(string payLoadFile, double expectedTotalCost, PowerPlantBasedOnMeritOrder sut)
        {
            var content = File.ReadAllText(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), payLoadFile));
            var payLoad = JsonSerializer.Deserialize<PayLoad>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
            //Act
            var combinationResult = await sut.ExecuteProcess(payLoad);
            //Assert
            Assert.Equal(expectedTotalCost, combinationResult.TotalCost);
        }

        [Theory]
        [InlineAutoData(@"JSONFiles\payload2.json")]
        [InlineAutoData(@"JSONFiles\payload10.json")]
        public async Task CalculateLoad_WhenCalled_ShouldThrowSystemException(string payLoadFile, Exception expectedException, PowerPlantBasedOnMeritOrder sut)
        {
            var content = File.ReadAllText(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), payLoadFile));
            var payLoad = JsonSerializer.Deserialize<PayLoad>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
            //Assert
            await Assert.ThrowsAsync<Exception>(async () => await sut.ExecuteProcess(payLoad));
        }
    }
}