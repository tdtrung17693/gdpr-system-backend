using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Web.Api.Core.Domain.Event;
using Web.Api.Core.Dto.Requests;
using Web.Api.Core.Interfaces.Gateways.Repositories;
using Web.Api.Core.Interfaces.Services.Event;
using Web.Api.Hubs;
using Web.Api.Infrastructure.Data.EntityFramework;

namespace Web.Api.EventHandlers
{
  public class NotifyRequestAcceptRejectStatus : IEventHandler<RequestAcceptedRejected>
  {
        private readonly IServiceProvider _provider;

        public NotifyRequestAcceptRejectStatus(IServiceProvider provider)
        {
            _provider = provider;
        }

        public async Task HandleAsync(RequestAcceptedRejected ev)
        {
            using (var scope = _provider.CreateScope())
            using (var ctx = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>())
            using (var notificationRepository = scope.ServiceProvider.GetRequiredService<INotificationRepository>())
            {
                await notificationRepository.CreateAcceptRejectNotification(new Requester()
                {
                    FullName = ev.RequesterFullName,
                    Username = ev.RequesterUsername,
                    UserId = ev.RequesterId,
                    Email = ev.RequesterEmail
                }, ev.NewStatus, ev.RequestTitle, ev.RequestId);
            }
        }
  }
}
