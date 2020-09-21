using System;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.Extensions.DependencyInjection;
using Web.Api.Core.Domain.Event;
using Web.Api.Core.Dto;
using Web.Api.Core.Dto.Requests;
using Web.Api.Core.Interfaces.Gateways.Repositories;
using Web.Api.Core.Interfaces.Services;
using Web.Api.Core.Interfaces.Services.Event;
using Web.Api.Infrastructure.Data.EntityFramework;

namespace Web.Api.EventHandlers
{
    public class NewRequestWebNotification : IEventHandler<RequestCreated>
    {
        private readonly IServiceProvider _provider;

        public NewRequestWebNotification(IServiceProvider provider)
        {
            _provider = provider;
        }

        public async Task HandleAsync(RequestCreated ev)
        {
            using (var scope = _provider.CreateScope())
            using (var ctx = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>())
            using (var notificationRepository = scope.ServiceProvider.GetRequiredService<INotificationRepository>())
            {
                var admin = await ctx.User.Include(user => user.Role)
                    .Where(user => user.Role.Name == "Administrator").ToListAsync();
                await notificationRepository.CreateNewRequestNotification(new Requester()
                {
                    FullName = ev.RequesterFullName,
                    Username = ev.RequesterUsername,
                    UserId = ev.RequesterId
                }, admin, ev.ServerName, ev.ServerId, ev.RequestId);
            }
        }
    }
}