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
using Web.Api.Core.Interfaces.UseCases.RequestInterface;

namespace Web.Api.Core.UseCases
{
    public sealed class CreateRequestUseCase : ICreateRequestUseCase
    {
        private readonly IRequestRepository _requestRepository;

        public CreateRequestUseCase(IRequestRepository requestRepository)
        {
            _requestRepository = requestRepository;
        }

        public async Task<bool> Handle(CreateRequestRequest message, IOutputPort<CreateRequestResponse> outputPort)
        {
            var response = await _requestRepository.CreateRequest(
                new Core.Domain.Entities.Request(message.Title, message.StartDate, message.EndDate, message.ServerId, message.Description,
                "New", "", null, Guid.NewGuid(), message.CreatedBy, DateTime.UtcNow ,null,null,null,null));

            return response.Success;
        }
        
    }
}
