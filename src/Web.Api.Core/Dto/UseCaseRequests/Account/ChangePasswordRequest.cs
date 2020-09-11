using System;
using System.Collections.Generic;
using System.Text;
using Web.Api.Core.Dto.UseCaseResponses.Account;
using Web.Api.Core.Interfaces;

namespace Web.Api.Core.Dto.UseCaseRequests.Account
{
  public class ChangePasswordRequest : IUseCaseRequest<ChangePasswordResponse>
  {
    public Guid UserId { get; }
    public string CurrentPassword { get; }
    public string NewPassword { get; }
    public IEnumerable<string> Errors { get; }

    public ChangePasswordRequest(Guid id, string currentPassword, string newPassword)
    {
      UserId = id;
      CurrentPassword = currentPassword;
      NewPassword = newPassword;
    }
  }
}
