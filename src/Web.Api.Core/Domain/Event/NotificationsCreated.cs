using System;
using System.Collections.Generic;
using System.Text;
using Web.Api.Core.Domain.Entities;
using Web.Api.Core.Dto;
using Web.Api.Core.Interfaces.Services.Event;

namespace Web.Api.Core.Domain.Event
{
  public class NotificationsCreated : IEvent
  {
    public  IEnumerable<NotificationDto> Notifications { get; }

    public NotificationsCreated(IEnumerable<NotificationDto> notifications)
    {
      Notifications = notifications;
    }
  }
}
