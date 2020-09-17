using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Api.Core.Domain.Event;
using Web.Api.Core.Interfaces.Services.Event;
using Web.Api.Hubs;

namespace Web.Api.EventHandlers
{
  public class BroadcastNewNotifications : IEventHandler<NotificationsCreated>
  {
    private readonly IHubContext<ConversationHub> _hubContext;
    public BroadcastNewNotifications(IHubContext<ConversationHub> hubContext)
    {
      _hubContext = hubContext;
    }
    public Task HandleAsync(NotificationsCreated ev)
    {
      var notifications = ev.Notifications;
      var tasks = new List<Task>();

      foreach(var notification in notifications)
      {
        tasks.Add(_hubContext.Clients.Group($"notification:{notification.ToUserId}").SendAsync("newNotification", notification));
      }

      Task.WhenAll(tasks);

      return Task.CompletedTask;
    }
  }
}
