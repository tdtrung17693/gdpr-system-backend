using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Web.Api.Core.Domain.Entities;
using Web.Api.Core.Dto.GatewayResponses.Repositories;

namespace Web.Api.Core.Interfaces.Gateways.Repositories
{
  public interface INotificationRepository
  {
    Task<bool> CreateNewRequestNotification(User creator, IEnumerable<User> recipients, string serverName, Guid serverId, Guid requestId);
    Task<IEnumerable<Notification>> GetNotificationOf(Guid userId);
    Task<UpdateNotificationResponse> MarkAsRead(Guid id);
    //Task<bool> CreateNewRequestNotification(User creator, User[] recipients, string content);
  }
}
