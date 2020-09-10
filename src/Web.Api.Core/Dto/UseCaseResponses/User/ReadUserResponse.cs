using System;
using System.Collections.Generic;
using Web.Api.Core.Interfaces;
using DomainEntities = Web.Api.Core.Domain.Entities;

namespace Web.Api.Core.Dto.UseCaseResponses.User
{
  public class ReadUserResponse : UseCaseResponseMessage
  {
    public Pagination<DomainEntities.User> Users { get; }
    public DomainEntities.User User { get; }
    public IEnumerable<Error> Errors { get; }

    public ReadUserResponse(IEnumerable<Error> errors, bool success = false, string message = null) : base(success, message)
    {
      Errors = errors;
    }

    public ReadUserResponse(Pagination<DomainEntities.User> users, bool success = true, string message = null) : base(success, message)
    {
      Users = users;
    }
    public ReadUserResponse(DomainEntities.User user, bool success = true, string message = null) : base(success, message)
    {
      User = user;
    }
  }
}
