using System;
using Web.Api.Core.Dto.UseCaseResponses;
using Web.Api.Core.Dto.UseCaseResponses.User;
using Web.Api.Core.Interfaces;

namespace Web.Api.Core.Dto.UseCaseRequests.User
{
  public class CreateUserRequest : IUseCaseRequest<CreateUserResponse>
  {
    public CreateUserRequest(string userName, string email, string firstName, string lastName, Guid roleId, string password)
    {
      FirstName = firstName;
      LastName = lastName;
      Username = userName;
      Email = email;
      RoleId = roleId;
      Password = password;
    }
    public string Username { get; set; }
    public string Email { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public Guid RoleId { get; set; }
    public string Password { get; set; }
  }
}
