using Web.Api.Core.Interfaces.Services.Event;

namespace Web.Api.Core.Domain.Event
{
  public class UserCreated : IEvent
  {
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string RawPassword { get; set; }
    public string Email { get; set; }
    public string Username { get; set; }

    public UserCreated(string firstName, string lastName, string rawPassword, string email, string username)
    {
      FirstName = firstName;
      LastName = lastName;
      Email = email;
      RawPassword = rawPassword;
      Username = username;
    }
  }
}
