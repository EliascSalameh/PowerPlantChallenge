using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.Xunit2;
using MediatR;
using Moq;
using REST.Engine.Entities;
using REST.Hosting.Controllers;
using REST.Services.Production;
using Xunit;

namespace REST.Tests
{
    public class PowerPlantControllerTests
    {
        private readonly Mock<IMediator> _mediatorMock = new();
        private readonly PowerPlantController _sut;

        public PowerPlantControllerTests()
        {
            _sut = new PowerPlantController(_mediatorMock.Object);
        }

        [Theory]
        [AutoData]
        public async Task ProductionPlan_WhenCalled_WillDelegateToMediator(
                                                                        CancellationToken token, PayLoad payload,
                                                                        List<ProductionResult> expected)
        {
            //ARRANGE
            _mediatorMock.Setup(m => m.Send(It.Is<GetProductionLoadRequest>(r => r.Payload == payload), token))
                .ReturnsAsync(expected);
            //Act
            var actual = await _sut.ProductionPlan(payload, token);
            //Assert
            Assert.Same(expected, actual);
        }
    }
}