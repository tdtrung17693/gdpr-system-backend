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
  public sealed class UpdateUserUseCase : IUpdateUserUseCase
  {
    private IUserRepository _userRepository;
    public UpdateUserUseCase(IUserRepository userRepository)
    {
      _userRepository = userRepository;
    }

    public async Task<bool> Handle(UpdateUserRequest message, IOutputPort<UpdateUserResponse> outputPort)
    {
      var response = await _userRepository.Update(new DomainEntities.User(message.FirstName, message.LastName, message.Email, message.RoleId));
      outputPort.Handle(response.Success ? new UpdateUserResponse(response.Id, true) : new UpdateUserResponse(response.Errors.Select(e => e.Description)));
      return response.Success;
    }
  }
}
