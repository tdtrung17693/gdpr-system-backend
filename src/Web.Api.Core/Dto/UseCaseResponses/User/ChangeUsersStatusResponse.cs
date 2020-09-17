using System;
using System.Collections.Generic;
using System.Text;
using Web.Api.Core.Interfaces;

namespace Web.Api.Core.Dto.UseCaseResponses.User
{
  public class ChangeUsersStatusResponse : UseCaseResponseMessage
  {
    public IEnumerable<string> Errors { get; }

    public ChangeUsersStatusResponse(IEnumerable<string> errors, bool success = false, string message = null) : base(success, message)
    {
      Errors = errors;
    }

    public ChangeUsersStatusResponse(bool success = true, string message = null) : base(success, message) { }
  }
}
