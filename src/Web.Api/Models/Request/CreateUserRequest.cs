using System;

namespace Web.Api.Models.Request 
{
  public class CreateUserRequest
  {
    public CreateUserRequest(string userName, string firstName, string lastName, string email, string roleId)
    {
      FirstName = firstName;
      LastName = lastName;
      Username = userName;
      Email = email;
      RoleId = roleId;
    }
    public string Username { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string RoleId { get; set; }
  }
}
