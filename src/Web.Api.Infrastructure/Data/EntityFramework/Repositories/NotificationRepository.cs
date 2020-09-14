using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Api.Core.Domain.Entities;
using Web.Api.Core.Dto;
using Web.Api.Core.Dto.GatewayResponses.Repositories;
using Web.Api.Core.Interfaces.Gateways.Repositories;
using Web.Api.Core.Interfaces.Services;

namespace Web.Api.Infrastructure.Data.EntityFramework.Repositories
{
  class NotificationRepository : INotificationRepository
  {
    private readonly ApplicationDbContext _context;
    private readonly User _currentUser;
    public NotificationRepository(ApplicationDbContext context, IAuthService authService)
    {
      _context = context;
      _currentUser = authService.GetCurrentUser();
    }
    public async Task<bool> CreateNewRequestNotification(User creator, IEnumerable<User> recipients, string serverName, Guid serverId, Guid requestId)
    {
      var newNotifications = recipients.Select(u => {
        var notifContent = JsonConvert.SerializeObject(new
        {
          creator.Account.Username,
          ServerName = serverName,
          ServerId = serverId,
          RequestId = requestId
        }, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

        var newNotif = new Notification(Guid.NewGuid(), creator.Id, (Guid)u.Id, "new-request", notifContent);

        return newNotif;
      });

      try
      {
        await _context.Notification.AddRangeAsync(newNotifications);
        await _context.SaveChangesAsync();
      } catch (Exception e)
      {
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
      Notification noti = _context.Notification.Where(n => n.Id == id).FirstOrDefault();
      if (noti == null) return new UpdateNotificationResponse(new[] { new Error(Error.Codes.ENTITY_NOT_FOUND, Error.Messages.ENTITY_NOT_FOUND) });
      if (_currentUser.Id != noti.ToUserId) return new UpdateNotificationResponse(new[] { new Error(Error.Codes.UNAUTHORIZED_ACCESS, Error.Messages.UNAUTHORIZED_ACCESS) });

      noti.IsRead = true;
      await _context.SaveChangesAsync();
      return new UpdateNotificationResponse();
    }
  }
}
