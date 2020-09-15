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
    public class GetRequestUseCase : IGetRequestUseCase
    {
        private readonly IRequestRepository _requestRepository;

        public GetRequestUseCase(IRequestRepository requestRepository)
        {
            _requestRepository = requestRepository;
        }

        public async Task<bool> Handle(GetRequestRequest message, IOutputPort<GetRequestResponse> outputPort)
        {
                var listRequests = await _requestRepository.GetRequest(message.PageNo, message.PageSize, message.Keyword, message.FilterStatus);
                var noPages = await _requestRepository.getNoPages(message.PageSize);
                outputPort.Handle(new GetRequestResponse(noPages, listRequests, true));
                return true;
        }
    }
}
