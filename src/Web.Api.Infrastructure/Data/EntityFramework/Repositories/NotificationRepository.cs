using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Web.Api.Core.Domain.Entities;
using Web.Api.Core.Domain.Event;
using Web.Api.Core.Dto;
using Web.Api.Core.Dto.GatewayResponses.Repositories;
using Web.Api.Core.Dto.Requests;
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
    private readonly IMapper _mapper;
    public NotificationRepository(ApplicationDbContext context, IAuthService authService, IDomainEventBus eventBus, IMapper mapper)
    {
      _context = context;
      _currentUser = authService.GetCurrentUser();
      _eventBus = eventBus;
      _mapper = mapper;
    }
    public async Task<bool> CreateNewRequestNotification(Requester requester, IEnumerable<User> recipients, string serverName, Guid serverId, Guid requestId)
    {
      var newNotificationTable = new DataTable();
      newNotificationTable.Columns.Add("Data", typeof(string));
      newNotificationTable.Columns.Add("FromUserId", typeof(Guid));
      newNotificationTable.Columns.Add("ToUserId", typeof(Guid));
      newNotificationTable.Columns.Add("NotificationType", typeof(string));

      foreach (var recipient in recipients)
      {
        var notifiedContent = JsonConvert.SerializeObject(
          new NewRequestNotificationData(
            requester.Username,
            serverName,
            serverId,
            requestId
          ),
          new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
        var newRow = newNotificationTable.NewRow();
        newRow["Data"] = notifiedContent;
        newRow["FromUserId"] = requester.UserId;
        newRow["ToUserId"] = recipient.Id;
        newRow["NotificationType"] = "new-request";
        newNotificationTable.Rows.Add(newRow);
      }

      var command = _context.Database.GetDbConnection().CreateCommand();
      command.CommandText = "CreateNotifications";
      command.CommandType = CommandType.StoredProcedure;
      command.Parameters.Add(new SqlParameter("@Notifications", newNotificationTable));

      if (command.Connection.State == ConnectionState.Closed)
          await command.Connection.OpenAsync();
      try
      {
        var reader = await command.ExecuteReaderAsync();
        var createdNotifications = _mapper.Map<IDataReader, List<NotificationDto>>(reader);
        _eventBus.Trigger(new NotificationsCreated(_mapper.Map<IEnumerable<NotificationDto>>(createdNotifications)));
      } catch (Exception e)
      {
        // Add log here
        Console.Error.WriteLine(e);
        return false;
      }

      return true;
    }

    public async Task<IEnumerable<Notification>> GetNotificationOf(Guid userId, int page, int pageSize = 5)
    {
      return await _context.Notification
          .Where(u => u.ToUserId == userId)
          .OrderByDescending(n => n.CreatedAt)
          .Skip(page * pageSize)
          .Take(pageSize)
          .ToListAsync();
    }

    public async Task<int> CountAllUnreadNotificationsOf(Guid userId)
    {
      return await _context.Notification.Where(u => u.ToUserId == userId && u.IsRead == false)
        .CountAsync();
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

    public async Task<UpdateNotificationResponse> MarkAllNotificationsOfUserAsRead(Guid userId)
    {
      return default(UpdateNotificationResponse);
    }
  }
}
