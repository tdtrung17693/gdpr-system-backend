using System.Collections.Generic;
using Web.Api.Core.Dto.UseCaseResponses.Account;
using Web.Api.Core.Interfaces;
using Web.Api.Core.Domain.Entities;

namespace Web.Api.Core.Dto.UseCaseRequests.Account
{
  public class UpdateProfileInfoRequest : IUseCaseRequest<UpdateProfileInfoResponse>
  {
    public Domain.Entities.User User { get; }
    public string FirstName { get; }
    public string LastName { get; }
    
    public UpdateProfileInfoRequest(Domain.Entities.User user, string firstName, string lastName)
    {
      User = user;
      FirstName = firstName;
      LastName = lastName;
    }
  }
}
