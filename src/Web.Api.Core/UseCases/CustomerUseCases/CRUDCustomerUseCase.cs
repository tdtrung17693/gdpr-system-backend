using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Api.Core.Domain.Entities;
using Web.Api.Core.Dto.GatewayResponses.Repositories;
using Web.Api.Core.Dto.UseCaseRequests;
using Web.Api.Core.Dto.UseCaseRequests.CustomerUseCaseRequest;
using Web.Api.Core.Dto.UseCaseResponses;
using Web.Api.Core.Interfaces;
using Web.Api.Core.Interfaces.Gateways.Repositories;
using Web.Api.Core.Interfaces.UseCases.ICustomerUseCases;
namespace Web.Api.Core.UseCases.CustomerUseCases
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
            Guid x;
            var isContactPointUID = Guid.TryParse(message.ContactPoint, out x);

            if (message.Id == null)
            {
                if (isContactPointUID)
                {
                    response = await _customerRepository.Create(new Customer(message.Name, message.ContractBeginDate, message.ContractEndDate,
                        Guid.Parse(message.ContactPoint), message.Description, message.Status, Guid.NewGuid()));
                }
                else
                {
                    response = await _customerRepository.CreateFromImport(message);
                }
            }
            else
            {
                response = await _customerRepository.Update(new Customer(message.Name, message.ContractBeginDate, message.ContractEndDate, 
                    Guid.Parse(message.ContactPoint), message.Description, message.Status, message.Id));
            }
            outputPort.Handle(response.Success ? new CustomerResponse(response.Id, true) : new CustomerResponse(response.Errors.Select(e => e.Description)));
            return response.Success;
        }
    }
}
