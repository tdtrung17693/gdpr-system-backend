using System;

namespace Web.Api.Models.Request 
{
  public class CreateUserRequest
  {
    public CreateUserRequest(string userName, string firstName, string lastName, string email, Guid roleId, string password, string confirmPassword)
    {
      FirstName = firstName;
      LastName = lastName;
      Username = userName;
      Email = email;
      RoleId = roleId;
      Password = password;
      ConfirmPassword = confirmPassword;
    }
    public string Username { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public Guid RoleId { get; set; }
    public string Password { get; set; }
    public string ConfirmPassword { get; set; }
  }
}
