using System;
using System.Collections.Generic;
using Web.Api.Core.Interfaces;

namespace Web.Api.Core.Dto.UseCaseResponses.User
{
  public class UpdateUserResponse : UseCaseResponseMessage
  {
    public IEnumerable<string> Errors { get; }

    public UpdateUserResponse(IEnumerable<string> errors, bool success = false, string message = null) : base(success, message)
    {
      Errors = errors;
    }

    public UpdateUserResponse(bool success, string message = null) : base(success, message)
    {
    }
  }
}
