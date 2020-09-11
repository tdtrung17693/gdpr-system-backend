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
using Web.Api.Core.Interfaces.UseCases.RequestInterface;

namespace Web.Api.Core.UseCases
{
    public sealed class BulkRequestUseCase : IBulkRequestUseCase
    {
        private readonly IRequestRepository _requestRepository;

        public BulkRequestUseCase(IRequestRepository requestRepository)
        {
            _requestRepository = requestRepository;
        }

        public async Task<bool> Handle(BulkRequestRequest message, IOutputPort<BulkRequestResponse> outputPort)
        { 
            var response = await _requestRepository.UpdateBulkRequestStatus(message.IdList, message.Status, message.Updator);
            //outputPort.Handle(response.Success ? new CreateNewRequestResponse(response.Id, true) : new CreateNewRequestResponse(response.Errors.Select(e => e.Description)));
            return response.Success;
        }
    }
}
