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
        await _eventBus.Trigger(new NotificationsCreated(_mapper.Map<IEnumerable<NotificationDto>>(createdNotifications)));
      } catch (Exception e)
      {
        // Add log here
        Console.Error.WriteLine(e);
        return false;
      }

      return true;
    }

    public async Task<Pagination<Notification>> GetNotificationOf(Guid userId, int page, int pageSize = 5)
    {
      var query = _context.Notification;
      var data = await query
        .Select(n => n)
        .Where(n => n.ToUserId == userId && (n.IsDeleted == false || n.IsDeleted == null))
        .OrderByDescending(n => n.CreatedAt)
        .Skip((page - 1) * pageSize)
        .Take(pageSize)
        .ToListAsync();
      var count = await query
        .Select(n => n)
        .Where(n => n.ToUserId == userId && (n.IsDeleted == false || n.IsDeleted == null))
        .CountAsync();

      
      return new Pagination<Notification>()
      {
        Items = data,
        TotalItems = count,
        TotalPages = (int) Math.Ceiling( count * 1.0 / pageSize),
        Page = page
      };
    }

    public async Task<int> CountAllUnreadNotificationsOf(Guid userId)
    {
      return await _context.Notification
        .Where(n => n.ToUserId == userId && n.IsRead == false && (n.IsDeleted == false || n.IsDeleted == null))
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
      var command = _context.Database.GetDbConnection().CreateCommand();
      command.CommandText = "MarkAllAsRead";
      command.CommandType = CommandType.StoredProcedure;
      command.Parameters.Add(new SqlParameter("@UserId", userId));

      if (command.Connection.State == ConnectionState.Closed)
          await command.Connection.OpenAsync();
      try
      {
        await command.ExecuteNonQueryAsync();
        return new UpdateNotificationResponse();
      }
      catch (Exception e)
      {
        Console.Error.WriteLine(e);
        return new UpdateNotificationResponse(new [] {new Error(Error.Codes.UNKNOWN, Error.Messages.UNKNOWN) });
      }
    }

    public async Task<UpdateNotificationResponse> Delete(Guid id)
    {
      var notification = _context.Notification.FirstOrDefault(n => n.Id == id);
      if (notification == null) return new UpdateNotificationResponse(new[] { new Error(Error.Codes.ENTITY_NOT_FOUND, Error.Messages.ENTITY_NOT_FOUND) });
      if (_currentUser.Id != notification.ToUserId) return new UpdateNotificationResponse(new[] { new Error(Error.Codes.UNAUTHORIZED_ACCESS, Error.Messages.UNAUTHORIZED_ACCESS) });

      notification.IsDeleted = true;
      notification.IsRead = true;
      await _context.SaveChangesAsync();
      return new UpdateNotificationResponse();
    }

    public async Task<IEnumerable<Notification>> GetNotificationOfUserToPage(Guid currentUserId, int page, int pageSize = 5)
    {
      var query = _context.Notification;
      var data = await query
        .Select(n => n)
        .Where(n => n.ToUserId == currentUserId && (n.IsDeleted == false || n.IsDeleted == null))
        .OrderByDescending(n => n.CreatedAt)
        .Take(pageSize * page)
        .ToListAsync();

      return data;
    }

    public void Dispose()
    {
      _context?.Dispose();
    }
  }
}
