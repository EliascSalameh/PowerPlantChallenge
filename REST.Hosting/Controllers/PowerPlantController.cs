using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using REST.Engine.Entities;
using REST.Services.Production;

namespace REST.Hosting.Controllers
{
    [ApiController]
    [Route("api")]
    public class PowerPlantController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PowerPlantController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Route("productionplan")]
        public Task<List<ProductionResult>> ProductionPlan([FromBody] PayLoad payLoad, CancellationToken token)
        {
            return _mediator.Send(new GetProductionLoadRequest(payLoad), token);
        }
    }
}