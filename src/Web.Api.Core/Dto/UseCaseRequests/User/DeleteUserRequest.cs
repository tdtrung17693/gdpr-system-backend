using System;
using Web.Api.Core.Dto.UseCaseResponses.User;
using Web.Api.Core.Interfaces;

namespace Web.Api.Core.Dto.UseCaseRequests.User
{
  public class DeleteUserRequest : IUseCaseRequest<DeleteUserResponse>
  {
        public DeleteUserRequest(string firstName, string lastName, string email, Guid roleId, string password)
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            RoleId = roleId;
            Password = password;
        }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public Guid RoleId { get; set; }
        public string Password { get; set; }
  }
}
