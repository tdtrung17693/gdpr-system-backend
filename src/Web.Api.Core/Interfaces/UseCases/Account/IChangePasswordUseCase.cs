using Web.Api.Core.Dto.UseCaseRequests.Account;
using Web.Api.Core.Dto.UseCaseResponses.Account;

namespace Web.Api.Core.Interfaces.UseCases.Account
{
  public interface IChangePasswordUseCase : IUseCaseRequestHandler<ChangePasswordRequest, ChangePasswordResponse>
  {
  }
}
