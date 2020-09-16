using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Web.Api.Core.Domain.Entities;
using Web.Api.Core.Dto;
using Web.Api.Core.Dto.GatewayResponses.Repositories;
using Web.Api.Core.Dto.Requests;

namespace Web.Api.Core.Interfaces.Gateways.Repositories
{
  public interface INotificationRepository
  {
    Task<bool> CreateNewRequestNotification(Requester requester, IEnumerable<User> recipients, string serverName, Guid serverId, Guid requestId);
    Task<IEnumerable<Notification>> GetNotificationOf(Guid userId, int page, int pageSize = 5);
    Task<int> CountAllUnreadNotificationsOf(Guid userId);
    Task<UpdateNotificationResponse> MarkAsRead(Guid id);

    Task<UpdateNotificationResponse> MarkAllNotificationsOfUserAsRead(Guid userId);
    //Task<bool> CreateNewRequestNotification(User creator, User[] recipients, string content);
  }
}
