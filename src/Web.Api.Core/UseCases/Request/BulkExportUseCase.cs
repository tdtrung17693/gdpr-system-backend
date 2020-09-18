using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Web.Api.Core.Domain.Entities;
using Web.Api.Core.Dto.UseCaseRequests;
using Web.Api.Core.Dto.UseCaseResponses;
using Web.Api.Core.Interfaces;
using Web.Api.Core.Interfaces.Gateways.Repositories;
using Web.Api.Core.Interfaces.UseCases.IRequestUseCases;

namespace Web.Api.Core.UseCases.Request
{
    public sealed class BulkExportUseCase : IBulkExportUseCase
    {
        private readonly IRequestRepository _requestRepository;

        public BulkExportUseCase(IRequestRepository requestRepository)
        {
            _requestRepository = requestRepository;
        }

        public async Task<bool> Handle(BulkExportRequest message, IOutputPort<BulkExportResponse> outputPort)
        {
            var res = await _requestRepository.exportBulkRequest(message);
            outputPort.Handle(new BulkExportResponse(res));
            return true;

        }
    }
}
