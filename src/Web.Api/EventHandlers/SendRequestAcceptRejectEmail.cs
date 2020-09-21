using System.Threading.Tasks;
using Web.Api.Core.Domain.Event;
using Web.Api.Core.Interfaces.Services;
using Web.Api.Core.Interfaces.Services.Event;

namespace Web.Api.EventHandlers
{
  public class SendRequestAcceptRejectEmail : IEventHandler<RequestAcceptedRejected>
  {
    private IMailService _mailService;
    public SendRequestAcceptRejectEmail(IMailService mailService)
    {
      _mailService = mailService;
    }
    public async Task HandleAsync(RequestAcceptedRejected ev)
    {
      var response = await _mailService.SendRequestStatusToRequester(ev.RequesterEmail, ev.RequesterFullName, ev.RequestId, ev.RequestTitle, ev.NewStatus);
    }
  }
}
