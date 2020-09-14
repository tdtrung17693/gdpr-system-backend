using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Api.Core.Domain.Entities;
using Web.Api.Core.Dto.UseCaseRequests;
using Web.Api.Core.Dto.UseCaseResponses;
using Web.Api.Core.Dto.UseCaseResponses.RequestUseCaseResponse;
using Web.Api.Core.Interfaces;
using Web.Api.Core.Interfaces.Gateways.Repositories;
using Web.Api.Core.Interfaces.UseCases;
using Web.Api.Core.Interfaces.UseCases.RequestInterface;

namespace Web.Api.Core.UseCases
{
    public sealed class UpdateRequestUseCase : IUpdateRequestUseCase
    {
        private readonly IRequestRepository _requestRepository;

        public UpdateRequestUseCase(IRequestRepository requestRepository)
        {
            _requestRepository = requestRepository;
        }

        public async Task<bool> Handle(UpdateRequestRequest message, IOutputPort<UpdateRequestResponse> outputPort)
        {
            var response = await _requestRepository.UpdateRequest(
                new Core.Domain.Entities.Request(message.Title, message.StartDate, message.EndDate, message.ServerId, message.Description,
                message.RequestStatus, message.Response, message.ApprovedBy, message.Id, Guid.NewGuid(), DateTime.UtcNow, message.UpdatedBy, DateTime.UtcNow, null, null));

            return response.Success;
        }
    }
}
