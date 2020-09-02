using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Api.Core.Domain.Entities;
using Web.Api.Core.Dto.UseCaseRequests;
using Web.Api.Core.Dto.UseCaseResponses;
using Web.Api.Core.Interfaces;
using Web.Api.Core.Interfaces.Gateways.Repositories;
using Web.Api.Core.Interfaces.UseCases;

namespace Web.Api.Core.UseCases
{
    public sealed class CreateServerUseCase : ICreateServerUseCase
    {
        private readonly IServerRepository _serverRepository;

        public CreateServerUseCase(IServerRepository serverRepository)
        {
            _serverRepository = serverRepository;
        }

        public async Task<bool> Handle(CreateServerRequest message, IOutputPort<CreateNewServerResponse> outputPort)//, IOutputPort<CreateNewServerResponse> outputPort
        {
            var response = await _serverRepository.Create(new Server(message.Id, message.CreatedAt, message.CreatedBy, message.UpdatedAt, message.UpdatedBy, message.DeletedAt, message.DeletedBy, message.IsDeleted, message.Status, message.Name, message.IpAddress, message.StartDate, message.EndDate));
            //outputPort.Handle(response.Success ? new CreateNewServerResponse(response.Id, true) : new CreateNewServerResponse(response.Errors.Select(e => e.Description)));
            return response.Success;
        }
    }
}
