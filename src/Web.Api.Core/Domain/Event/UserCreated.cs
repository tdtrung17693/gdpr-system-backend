using Web.Api.Core.Interfaces.Services.Event;

namespace Web.Api.Core.Domain.Event
{
  public class UserCreated : IEvent
  {
    public string FirstName { get; }
    public string LastName { get; }
    public string RawPassword { get; }
    public string Email { get; }
    public string Username { get; }

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
