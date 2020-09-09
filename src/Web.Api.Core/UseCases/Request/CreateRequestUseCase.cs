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
            var response = await _requestRepository.Create(new Request(message.Title, message.StartDate, message.EndDate,
                message.ServerId, message.Description, message.RequestStatus, /*message.Response, message.ApprovedBy,*/
                message.Id, message.CreatedBy, message.CreatedAt/*, message.UpdatedBy, message.UpdatedAt, message.DeletedBy, message.DeletedAt*/));
            return response.Success;
        }
    }
}
