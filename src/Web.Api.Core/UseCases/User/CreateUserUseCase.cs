using System;
using System.Linq;
using System.Threading.Tasks;
using Web.Api.Core.Dto.UseCaseRequests;
using Web.Api.Core.Dto.UseCaseRequests.User;
using Web.Api.Core.Dto.UseCaseResponses;
using Web.Api.Core.Dto.UseCaseResponses.User;
using Web.Api.Core.Interfaces;
using Web.Api.Core.Interfaces.Gateways.Repositories;
using Web.Api.Core.Interfaces.UseCases;
using Web.Api.Core.Interfaces.UseCases.User;
using DomainEntities = Web.Api.Core.Domain.Entities;

namespace Web.Api.Core.UseCases.User
{
  public sealed class CreateUserUseCase : ICreateUserUseCase
  {
    private IUserRepository _userRepository;
    public CreateUserUseCase(IUserRepository userRepository)
    {
      _userRepository = userRepository;
    }

    public async Task<bool> Handle(CreateUserRequest message, IOutputPort<CreateUserResponse> outputPort)
    {
      var response = await _userRepository.Create(new DomainEntities.User(message.FirstName, message.LastName, message.Email, message.RoleId), message.Password);
      outputPort.Handle(response.Success ? new CreateUserResponse(response.Id, true) : new CreateUserResponse(response.Errors.Select(e => e.Description)));
      return response.Success;
    }
  }
}
