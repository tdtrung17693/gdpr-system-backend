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
    public class ExportUseCase : IExportUseCase
    {
        private readonly IRequestRepository _requestRepository;

        public ExportUseCase(IRequestRepository requestRepository)
        {
            _requestRepository = requestRepository;
        }

        public async Task<bool> Handle(ExportRequest message, IOutputPort<ExportResponse> outputPort)
        {
            var listRequests = await _requestRepository.GetRequestForExport(message);
            outputPort.Handle(new ExportResponse(listRequests));
            return true;
        }
    }
}
