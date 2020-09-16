using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Web.Api.Core.Dto.UseCaseRequests;
using Web.Api.Core.Dto.UseCaseResponses;
using Web.Api.Core.Interfaces;
using Web.Api.Core.Interfaces.Gateways.Repositories;
using Web.Api.Core.Interfaces.UseCases;

namespace Web.Api.Core.UseCases.Request
{
    public class GetEachRequestUseCase : IGetEachRequestUseCase
    {
        private readonly IRequestRepository _requestRepository;

        public GetEachRequestUseCase(IRequestRepository requestRepository)
        {
            _requestRepository = requestRepository;
        }

        public async Task<bool> Handle(GetEachRequestRequest message, IOutputPort<GetEachRequestResponse> outputPort)
        {
            var request = _requestRepository.getEachRequest(message.RequestId);
            outputPort.Handle(new GetEachRequestResponse(request, true));
            return true;
        }
    }
}
