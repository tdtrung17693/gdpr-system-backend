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
using Web.Api.Core.Interfaces.UseCases;

namespace Web.Api.Core.UseCases
{
    public sealed class CreateServerUseCase : ICreateServerUseCase
    {
        private readonly IServerRepository _serverRepository;

        public CreateServerUseCase(IServerRepository serverRepository)
        {
            _serverRepository = serverRepository;
        }

        public async Task<bool> Handle(CreateServerRequest message, IOutputPort<CreateNewServerResponse> outputPort)
        {
            var response = await _serverRepository.Create(new Server(message.Id, message.CreatedBy, message.CreatedAt, message.UpdatedBy, message.UpdatedAt, message.DeletedBy, message.DeletedAt, message.isDeleted, message.Name, message.IpAddress, message.StartDate, message.EndDate));
            outputPort.Handle(response.Success ? new CreateNewServerResponse(response.Id, true) : new CreateNewServerResponse(response.Errors.Select(e => e.Description)));
            return response.Success;
        }

        //public async Task<bool> Handle(RegisterUserRequest message, IOutputPort<RegisterUserResponse> outputPort)
        //{
        //    var response = await _userRepository.Create(new User(message.FirstName, message.LastName, message.Email, message.UserName), message.Password);
        //    outputPort.Handle(response.Success ? new RegisterUserResponse(response.Id, true) : new RegisterUserResponse(response.Errors.Select(e => e.Description)));
        //    return response.Success;
        //}
    }
}
