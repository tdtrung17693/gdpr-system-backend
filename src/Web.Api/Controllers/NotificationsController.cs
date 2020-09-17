using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web.Api.Core.Domain.Entities;
using Web.Api.Core.Dto;
using Web.Api.Core.Dto.UseCaseRequests;
using Web.Api.Core.Interfaces.Gateways.Repositories;
using Web.Api.Core.Interfaces.Services;
using Web.Api.Core.Interfaces.UseCases;
using Web.Api.Models.Request;
using Web.Api.Presenters;

namespace Web.Api.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class NotificationsController : ControllerBase
  {
    private readonly User _currentUser;
    private readonly INotificationRepository _notificationRepository;

    public NotificationsController(IAuthService authService, INotificationRepository notificationRepository)
    {
      _currentUser = authService.GetCurrentUser();
      _notificationRepository = notificationRepository;
    }

    [HttpGet("more")]
    [Authorize()]
    public async Task<Pagination<Notification>> GetNotifications([FromQuery] GetNotificationsRequest request)
    {
      if (request.Page < 0)
      {
        request.Page = 1;
      }
      
      var notifications = await _notificationRepository.GetNotificationOf((System.Guid)_currentUser.Id, request.Page);
      return notifications;
    }

    [HttpPut("{id}")]
    [Authorize()]
    public async Task<IActionResult> MarkAsRead(Guid id)
    {
      var response = await _notificationRepository.MarkAsRead(id);

      if (response.Success) return Ok();
      var firstError = response.Errors.First();

      switch (firstError.Code)
      {
        case Error.Codes.ENTITY_NOT_FOUND:
          return NotFound(firstError.Description);
        case Error.Codes.UNAUTHORIZED_ACCESS:
          return  StatusCode((int)HttpStatusCode.Unauthorized, firstError.Description);
      }

      return BadRequest();
    }
    
    [HttpPut("mark-read-all")]
    [Authorize()]
    public async Task<IActionResult> MarkAllAsRead()
    {
      var response = await _notificationRepository.MarkAllNotificationsOfUserAsRead((Guid) _currentUser.Id);

      if (response.Success) return Ok();

      return BadRequest(response.Errors);
    }
    
    [HttpDelete("{id}")]
    [Authorize()]
    public async Task<IActionResult> Delete(Guid id)
    {
      var response = await _notificationRepository.Delete(id);

      if (response.Success) return Ok();
      var firstError = response.Errors.First();

      switch (firstError.Code)
      {
        case Error.Codes.ENTITY_NOT_FOUND:
          return NotFound(firstError.Description);
        case Error.Codes.UNAUTHORIZED_ACCESS:
          return  StatusCode((int)HttpStatusCode.Unauthorized, firstError.Description);
      }

      return BadRequest();
    }
  }
}
