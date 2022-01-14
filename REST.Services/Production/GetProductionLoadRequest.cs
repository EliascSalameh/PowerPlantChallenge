using System.Collections.Generic;
using MediatR;
using REST.Engine.Entities;

namespace REST.Services.Production
{
    public class GetProductionLoadRequest : IRequest<List<ProductionResult>>
    {
        public readonly PayLoad Payload;

        public GetProductionLoadRequest(PayLoad payLoad)
        {
            Payload = payLoad;
        }
    }
}