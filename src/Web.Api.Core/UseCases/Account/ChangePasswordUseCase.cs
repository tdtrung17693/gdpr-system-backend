using System.Threading.Tasks;
using Web.Api.Core.Dto;
using Web.Api.Core.Dto.UseCaseRequests.Account;
using Web.Api.Core.Dto.UseCaseResponses.Account;
using Web.Api.Core.Interfaces;
using Web.Api.Core.Interfaces.Gateways.Repositories;
using Web.Api.Core.Interfaces.UseCases.Account;

namespace Web.Api.Core.UseCases.Account
{
  public sealed class ChangePasswordUseCase : IChangePasswordUseCase
  {
    private readonly IUserRepository _userRepository;
    public ChangePasswordUseCase(IUserRepository userRepository)
    {
      _userRepository = userRepository;
    }

    public async Task<bool> Handle(ChangePasswordRequest message, IOutputPort<ChangePasswordResponse> outputPort)
    {
      var user = message.User;
      var currentPassword = message.CurrentPassword;
      var newPassword = message.NewPassword;

      var passwordValid = await _userRepository.CheckPassword(user, currentPassword);
      if (!passwordValid)
      {
        outputPort.Handle(
          new ChangePasswordResponse(new[] {new Error(Error.Codes.INVALID_CREDENTIAL, Error.Messages.INVALID_CREDENTIAL), })
          );
        return false;
      }

      var response = await _userRepository.ChangePassword(user, newPassword);
      if (!response.Success)
      {
        outputPort.Handle(new ChangePasswordResponse(response.Errors));
      }
      
      outputPort.Handle(
        new ChangePasswordResponse()
        );
      
      return true;
    }
  }
}
