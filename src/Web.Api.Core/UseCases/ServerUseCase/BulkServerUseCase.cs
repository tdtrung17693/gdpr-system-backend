using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Api.Core.Domain.Entities;
using Web.Api.Core.Dto.UseCaseRequests;
using Web.Api.Core.Dto.UseCaseRequests.ServerUserCaseRequest;
using Web.Api.Core.Dto.UseCaseResponses;
using Web.Api.Core.Dto.UseCaseResponses.ServerUseCaseResponse;
using Web.Api.Core.Interfaces;
using Web.Api.Core.Interfaces.Gateways.Repositories;
using Web.Api.Core.Interfaces.UseCases.ServerInterface;

namespace Web.Api.Core.UseCases
{
    public sealed class BulkServerUseCase : IBulkServerUseCase
    {
        private readonly IServerRepository _serverRepository;

        public BulkServerUseCase(IServerRepository serverRepository)
        {
            _serverRepository = serverRepository;
        }

        public async Task<bool> Handle(BulkServerRequest message, IOutputPort<BulkServerResponse> outputPort)//, IOutputPort<CreateNewServerResponse> outputPort
        {
            //UpdateMutilServerStatus( DataTable serverIdList, bool status, Guid userId)
            var response = await _serverRepository.UpdateMutilServerStatus(message.IdList, message.Status, message.Updator );
            //outputPort.Handle(response.Success ? new CreateNewServerResponse(response.Id, true) : new CreateNewServerResponse(response.Errors.Select(e => e.Description)));
            return response.Success;
        }
    }
}
