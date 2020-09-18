using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Web.Api.Core.Domain.Event;
using Web.Api.Core.Interfaces.Gateways.Repositories;
using Web.Api.Core.Interfaces.Services.Event;
using Web.Api.Infrastructure.Data.EntityFramework;

namespace Web.Api.EventHandlers
{
    public class LogRequestUpdated : IEventHandler<RequestUpdated>
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogRepository _logRepo;
        public LogRequestUpdated(ApplicationDbContext context, ILogRepository logRepo)
        {
            _context = context;
            _logRepo = logRepo;
        }

        public async Task HandleAsync(RequestUpdated ev)
        {
            var updator = await _context.User.Where(user => user.Id == ev.UpdatedBy).ToListAsync();
            await _logRepo.LogUpdateRequest(ev.RequestId, ev.UpdatedFields, updator[0]);
        }
    }
}