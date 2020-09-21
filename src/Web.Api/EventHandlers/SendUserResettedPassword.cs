using System.Threading.Tasks;
using Web.Api.Core.Domain.Event;
using Web.Api.Core.Interfaces.Services;
using Web.Api.Core.Interfaces.Services.Event;

namespace Web.Api.EventHandlers
{
    public class SendUserResettedPassword : IEventHandler<UserPasswordResetted>
    {
        private IMailService _mailService;

        public SendUserResettedPassword(IMailService mailService)
        {
            _mailService = mailService;
        }
        public async Task HandleAsync(UserPasswordResetted ev)
        {
            await _mailService.SendResettedPassword(ev.UserEmail, ev.UserFullName, ev.NewPassword);
        }
    }
}