using System.Threading.Tasks;
using Web.Api.Core.Domain.Event;
using Web.Api.Core.Interfaces.Services;
using Web.Api.Core.Interfaces.Services.Event;

namespace Web.Api.EventHandlers
{
  public class SendInviteMail : IEventHandler<UserCreated>
  {
    private IMailService _mailService;
    public SendInviteMail(IMailService mailService)
    {
      _mailService = mailService;
    }
    public async Task HandleAsync(UserCreated ev)
    {
      var response = await _mailService.SendInvitedNotification(ev.Email, ev.Username, ev.RawPassword, ev.FirstName, ev.LastName);
    }
  }
}
