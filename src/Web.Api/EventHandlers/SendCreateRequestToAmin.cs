using Microsoft.EntityFrameworkCore;
using System.Data.SqlTypes;
using System.Linq;
using System.Threading.Tasks;
using Web.Api.Core.Domain.Event;
using Web.Api.Core.Interfaces.Services;
using Web.Api.Core.Interfaces.Services.Event;
using Web.Api.Infrastructure.Data.EntityFramework;

namespace Web.Api.EventHandlers
{
    public class SendCreateRequestToAmin : IEventHandler<RequestNotiToAdmin>
    {
        private IMailService _mailService;
        private readonly ApplicationDbContext _context;
        public SendCreateRequestToAmin(ApplicationDbContext context, IMailService mailService)
        {
            _mailService = mailService;
            _context = context;
        }
        public async Task HandleAsync(RequestNotiToAdmin ev)
        {
            var adminList = await _context.User.Include(user => user.Role).Where(user => user.Role.Name == "Administrator").ToListAsync();
            var response = await _mailService.SendCreatedRequestToAdmin(adminList, ev.RequesterFullName, ev.ServerName, ev.RequestId, ev.ServerId, ev.CreatedAt);
        }
    }
}
