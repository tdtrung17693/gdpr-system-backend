using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Api.Core.Domain.Entities;
using Web.Api.Core.Dto.GatewayResponses.Repositories;
using Web.Api.Core.Dto.UseCaseRequests;
using Web.Api.Core.Dto.UseCaseResponses;
using Web.Api.Core.Interfaces;
using Web.Api.Core.Interfaces.Gateways.Repositories;
using Web.Api.Core.Interfaces.UseCases;

namespace Web.Api.Core.UseCases
{
    public sealed class CRUDCustomerUseCase: ICRUDCustomerUseCase
    {
        private readonly ICustomerRepository _customerRepository;

        public CRUDCustomerUseCase(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task<bool> Handle(CustomerRequest message, IOutputPort<CustomerResponse> outputPort)
        {
            CRUDCustomerResponse response;

            if (message.Id == null)
            {
                response = await _customerRepository.Create(new Customer(message.Name, message.ContractBeginDate, message.ContractEndDate, message.ContactPoint, message.Description, message.Status, Guid.NewGuid()));
            }
            else
            {
                response = await _customerRepository.Update(new Customer(message.Name, message.ContractBeginDate, message.ContractEndDate, message.ContactPoint, message.Description, message.Status, message.Id));
            }
            outputPort.Handle(response.Success ? new CustomerResponse(response.Id, true) : new CustomerResponse(response.Errors.Select(e => e.Description)));
            return response.Success;
        }
    }
}
