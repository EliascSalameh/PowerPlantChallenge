using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using REST.Engine.Entities;
using REST.Engine.Interface;

namespace REST.Services.Production
{
    public class GetProductionLoadHandler : IRequestHandler<GetProductionLoadRequest, List<ProductionResult>>
    {
        private readonly IPowerPlantStrategy _engine;

        public GetProductionLoadHandler(IPowerPlantStrategy engine)
        {
            this._engine = engine ?? throw new ArgumentNullException(nameof(engine));
        }

        public async Task<List<ProductionResult>> Handle(GetProductionLoadRequest request,
            CancellationToken cancellationToken)
        {
            return (await _engine.ExecuteProcess(request.Payload)).ProductionResults;
        }
    }
}