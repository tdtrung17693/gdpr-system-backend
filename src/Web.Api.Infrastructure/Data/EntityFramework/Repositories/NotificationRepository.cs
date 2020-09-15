using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Api.Core.Domain.Entities;
using Web.Api.Core.Domain.Event;
using Web.Api.Core.Dto;
using Web.Api.Core.Dto.GatewayResponses.Repositories;
using Web.Api.Core.Interfaces.Gateways.Repositories;
using Web.Api.Core.Interfaces.Services;
using Web.Api.Core.Interfaces.Services.Event;

namespace Web.Api.Infrastructure.Data.EntityFramework.Repositories
{
  public class NotificationRepository : INotificationRepository
  {
    private readonly ApplicationDbContext _context;
    private readonly User _currentUser;
    private readonly IDomainEventBus _eventBus;
    public NotificationRepository(ApplicationDbContext context, IAuthService authService, IDomainEventBus eventBus)
    {
      _context = context;
      _currentUser = authService.GetCurrentUser();
      _eventBus = eventBus;
    }
    public async Task<bool> CreateNewRequestNotification(User creator, IEnumerable<User> recipients, string serverName, Guid serverId, Guid requestId)
    {
      var newNotifications = recipients.Select(u => {
        var notifiedContent = JsonConvert.SerializeObject(new
        {
          creator.Account.Username,
          ServerName = serverName,
          ServerId = serverId,
          RequestId = requestId
        }, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

        var newNotification = new Notification(Guid.NewGuid(), creator.Id, (Guid) u.Id, "new-request", notifiedContent,createdAt:DateTime.UtcNow);


        return newNotification;
      });

      try
      {
        await _context.Notification.AddRangeAsync(newNotifications);
        await _context.SaveChangesAsync();
        _eventBus.Trigger(new NotificationsCreated(newNotifications));
      } catch (Exception e)
      {
        // Add log here
        Console.Error.WriteLine(e);
        return false;
      }

      return true;
    }

    public async Task<IEnumerable<Notification>> GetNotificationOf(Guid userId)
    {
      return await _context.Notification.Where(n => n.ToUserId == userId && n.IsRead == false).ToListAsync();
    }

    public async Task<UpdateNotificationResponse> MarkAsRead(Guid id)
    {
      var notification = _context.Notification.FirstOrDefault(n => n.Id == id);
      if (notification == null) return new UpdateNotificationResponse(new[] { new Error(Error.Codes.ENTITY_NOT_FOUND, Error.Messages.ENTITY_NOT_FOUND) });
      if (_currentUser.Id != notification.ToUserId) return new UpdateNotificationResponse(new[] { new Error(Error.Codes.UNAUTHORIZED_ACCESS, Error.Messages.UNAUTHORIZED_ACCESS) });

      notification.IsRead = true;
      await _context.SaveChangesAsync();
      return new UpdateNotificationResponse();
    }
  }
}
