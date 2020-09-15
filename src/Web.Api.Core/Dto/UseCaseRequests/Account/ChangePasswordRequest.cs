using System;
using System.Collections.Generic;
using System.Text;
using Web.Api.Core.Dto.UseCaseResponses.Account;
using Web.Api.Core.Interfaces;

namespace Web.Api.Core.Dto.UseCaseRequests.Account
{
  public class ChangePasswordRequest : IUseCaseRequest<ChangePasswordResponse>
  {
    public Domain.Entities.User User { get; }
    public string CurrentPassword { get; }
    public string NewPassword { get; }

    public ChangePasswordRequest(Domain.Entities.User user, string currentPassword, string newPassword)
    {
      User = user;
      CurrentPassword = currentPassword;
      NewPassword = newPassword;
    }
  }
}
