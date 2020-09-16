using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Web.Api.Core.Dto.UseCaseRequests;
using Web.Api.Core.Dto.UseCaseResponses;
using Web.Api.Core.Interfaces;
using Web.Api.Core.Interfaces.Gateways.Repositories;
using Web.Api.Core.Interfaces.UseCases;

namespace Web.Api.Core.UseCases
{
    public class ManageRequestUseCase : IManageRequestUseCase
    {
        private readonly IRequestRepository _requestRepository;


        public ManageRequestUseCase(IRequestRepository requestRepository)
        {
            _requestRepository = requestRepository;
        }

        public async Task<bool> Handle(ManageRequestRequest message, IOutputPort<ManageRequestResponse> outputPort)
        {
            var manageRequestResult = await _requestRepository.ManageRequest(message);
            outputPort.Handle(new ManageRequestResponse(true, null));
            return true;
        }
    }
}
