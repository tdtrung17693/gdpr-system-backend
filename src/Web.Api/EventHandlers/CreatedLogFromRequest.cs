using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Api.Core.Domain.Event;
using Web.Api.Core.Interfaces.Gateways.Repositories;
using Web.Api.Core.Interfaces.Services.Event;
using Web.Api.Infrastructure.Data.EntityFramework;

namespace Web.Api.EventHandlers
{
    public class CreatedLogFromRequest : IEventHandler<CreateLog>
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogRepository _logRepo;
        public CreatedLogFromRequest(ApplicationDbContext context, ILogRepository logRepo)
        {
            _context = context;
            _logRepo = logRepo;
        }
        public async Task HandleAsync(CreateLog ev)
        {
            var creator = await _context.User.Where(user => user.Id == ev.CreatedBy).ToListAsync();
            var role = await _context.Role.Where(role => role.Id == creator[0].RoleId).ToListAsync();
            ev.UpdatedField = role[0].Name;
            await _logRepo.Create(ev, creator[0]);

        }
    }
}
