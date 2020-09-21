using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Api.Core.Dto.GatewayResponses.Repositories;
using Web.Api.Core.Dto.UseCaseRequests;
using Web.Api.Core.Dto.UseCaseRequests.CustomerUseCaseRequest;
using Web.Api.Core.Dto.UseCaseResponses;
using Web.Api.Core.Interfaces;
using Web.Api.Core.Interfaces.Gateways.Repositories;
using Web.Api.Core.Interfaces.UseCases.ICustomerUseCases;

namespace Web.Api.Core.UseCases.CustomerUseCases
{
    public sealed class ExportCustomerUseCase: IExportCustomerUseCase
    {
        private readonly ICustomerRepository _customerRepository;

        public ExportCustomerUseCase(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }
        public async Task<bool> Handle(ExportCustomerRequest message, IOutputPort<ExportCustomerResponse> outputPort)
        {
            ExportCSVByCustomerResponse response;

            response = await _customerRepository.GetByCustomers(message);

            outputPort.Handle(response.Success ? new ExportCustomerResponse(response.ResponsedRequest, true) :
                new ExportCustomerResponse(response.Errors.Select(e => e.Description)));
            return response.Success;
        }
    }
}
