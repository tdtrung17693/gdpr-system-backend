using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Data.SqlTypes;
using System.Linq;
using System.Threading.Tasks;
using Web.Api.Core.Domain.Entities;
using Web.Api.Core.Domain.Event;
using Web.Api.Core.Interfaces.Services;
using Web.Api.Core.Interfaces.Services.Event;
using Web.Api.Infrastructure.Data.EntityFramework;

namespace Web.Api.EventHandlers
{
    public class SendCreateRequestToAmin : IEventHandler<RequestCreated>
    {
        private IMailService _mailService;
        private readonly IServiceProvider _provider;
        public SendCreateRequestToAmin(IServiceProvider provider, IMailService mailService)
        {
            _mailService = mailService;
            _provider = provider;
        }
        public async Task HandleAsync(RequestCreated ev)
        {
          using (var scope = _provider.CreateScope())
          using (var ctx = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>())
          {
            var adminList = await ctx.User.Include(user => user.Role).Where(user => user.Role.Name == "Administrator").ToListAsync();
            var response = await _mailService.SendCreatedRequestToAdmin(adminList, ev.RequesterFullName, ev.ServerName, ev.RequestId, ev.ServerId, ev.CreatedAt);
          }
        }
    }
}
