using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Api.Core.Domain.Entities;
using Web.Api.Core.Domain.Event;
using Web.Api.Core.Interfaces.Gateways.Repositories;
using Web.Api.Core.Interfaces.Services.Event;
using Web.Api.Infrastructure.Data.EntityFramework;

namespace Web.Api.EventHandlers
{
    public class LogNewRequest : IEventHandler<RequestCreated>
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogRepository _logRepo;
        public LogNewRequest(ApplicationDbContext context, ILogRepository logRepo)
        {
            _context = context;
            _logRepo = logRepo;
        }
        public async Task HandleAsync(RequestCreated ev)
        {
            var creator = await _context.User.Where(user => user.Id == ev.RequesterId).ToListAsync();
            await _logRepo.LogNewRequest(ev.RequestId, creator[0]);
        }
    }
}
