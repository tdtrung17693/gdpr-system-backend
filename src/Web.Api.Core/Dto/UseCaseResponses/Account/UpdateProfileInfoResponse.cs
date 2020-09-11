using System;
using System.Collections.Generic;
using Web.Api.Core.Interfaces;

namespace Web.Api.Core.Dto.UseCaseResponses.Account
{
  public class UpdateProfileInfoResponse : UseCaseResponseMessage
  {
    public IDictionary<string, string> UpdatedFields { get; }
    public IEnumerable<Error> Errors { get; }

    public UpdateProfileInfoResponse(IEnumerable<Error> errors, bool success = false, string message = null) : base(success, message)
    {
      Errors = errors;
    }

    public UpdateProfileInfoResponse(IDictionary<string, string> updatedFields, bool success = true, string message = null) : base(success, message)
    {
      UpdatedFields = updatedFields;
    }
  }
}
