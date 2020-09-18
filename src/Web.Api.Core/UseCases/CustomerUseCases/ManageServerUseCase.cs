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
    public sealed class ManageServerUseCase: IManageServerUseCase
    {
        private readonly ICustomerRepository _customerRepository;

        public ManageServerUseCase(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task<bool> Handle(ManageServerRequest message, IOutputPort<ManageServerResponse> outputPort)
        {
            ManageServerCustomerResponse response;
            if (message.Action) {
                response = await _customerRepository.AddServerOwner(message);
            }
            else { 
                response = await _customerRepository.RemoveServerOwner(message);
            }
            outputPort.Handle(response.Success ? new ManageServerResponse(true) : new ManageServerResponse(response.Errors.Select(e => e.Description)));
            return response.Success;
        }
    }
}
