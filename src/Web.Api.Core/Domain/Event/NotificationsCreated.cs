using System;
using System.Collections.Generic;
using System.Text;
using Web.Api.Core.Domain.Entities;
using Web.Api.Core.Interfaces.Services.Event;

namespace Web.Api.Core.Domain.Event
{
  public class NotificationsCreated : IEvent
  {
    public  IEnumerable<Notification> Notifications { get; }

    public NotificationsCreated(IEnumerable<Notification> notifications)
    {
      Notifications = notifications;
    }
  }
}
