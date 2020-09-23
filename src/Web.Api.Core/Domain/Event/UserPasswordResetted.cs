using Web.Api.Core.Interfaces.Services.Event;

namespace Web.Api.Core.Domain.Event
{
  public class UserPasswordResetted : IEvent
  {
    public string UserEmail { get; set; }
    public string UserFullName { get; set; }
    public string NewPassword { get; set; }

    public UserPasswordResetted(string email, string userFullName, string newPassword)
    {
      UserEmail = email;
      UserFullName = userFullName;
      NewPassword = newPassword;
    }
  }
}
