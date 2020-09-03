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
  public sealed class DeleteUserUseCase : IDeleteUserUseCase
  {
    private IUserRepository _userRepository;
    public DeleteUserUseCase(IUserRepository userRepository)
    {
      _userRepository = userRepository;
    }

    public async Task<bool> Handle(DeleteUserRequest message, IOutputPort<DeleteUserResponse> outputPort)
    {
      var response = await _userRepository.Delete(new DomainEntities.User(message.FirstName, message.LastName, message.Email, message.RoleId));
      outputPort.Handle(response.Success ? new DeleteUserResponse(response.Id, true) : new DeleteUserResponse(response.Errors.Select(e => e.Description)));
      return response.Success;
    }
  }
}
