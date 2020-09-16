using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Api.Core.Dto.GatewayResponses.Repositories;
using Web.Api.Core.Dto.UseCaseRequests;
using Web.Api.Core.Dto.UseCaseResponses;
using Web.Api.Core.Interfaces;
using Web.Api.Core.Interfaces.Gateways.Repositories;
using Web.Api.Core.Interfaces.UseCases;

namespace Web.Api.Core.UseCases
{
    public sealed class ExportServerUseCase : IExportServerUseCase
    {
        private readonly IServerRepository _serverRepository;

        public ExportServerUseCase(IServerRepository serverRepository)
        {
            _serverRepository = serverRepository;
        }
        public async Task<bool> Handle(ExportServerRequest message, IOutputPort<ExportServerResponse> outputPort)
        {
            ExportCSVByCustomerResponse response;

            response = await _serverRepository.GetExportServers(message);

            outputPort.Handle(response.Success ? new ExportServerResponse(response.ResponsedRequest, true) :
                new ExportServerResponse(response.Errors.Select(e => e.Description)));
            return response.Success;
        }
    }
}
