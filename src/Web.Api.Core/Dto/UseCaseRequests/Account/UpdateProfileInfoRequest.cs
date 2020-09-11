using System;
using System.Collections.Generic;
using System.Text;
using Web.Api.Core.Dto.UseCaseResponses.Account;
using Web.Api.Core.Interfaces;

namespace Web.Api.Core.Dto.UseCaseRequests.Account
{
  public class UpdateProfileInfoRequest : IUseCaseRequest<UpdateProfileInfoResponse>
  {
    public Guid UserId { get; }
    public string FirstName { get; }
    public string LastName { get; }
    public IEnumerable<string> Errors { get; }

    public UpdateProfileInfoRequest(Guid id, string firstName, string lastName)
    {
      UserId = id;
      FirstName = firstName;
      LastName = lastName;
    }
  }
}
