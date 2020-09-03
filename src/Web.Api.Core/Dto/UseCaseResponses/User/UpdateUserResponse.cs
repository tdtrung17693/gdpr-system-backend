using System;
using System.Collections.Generic;
using Web.Api.Core.Interfaces;
using DomainEntities = Web.Api.Core.Domain.Entities;

namespace Web.Api.Core.Dto.UseCaseResponses.User
{
  public class UpdateUserResponse : UseCaseResponseMessage
  {
    public string Id { get; }
    public IEnumerable<string> Errors { get; }

    public UpdateUserResponse(IEnumerable<string> errors, bool success = false, string message = null) : base(success, message)
    {
      Errors = errors;
    }

    public UpdateUserResponse(string id, bool success = false, string message = null) : base(success, message)
    {
      Id = id;
    }
  }
}
