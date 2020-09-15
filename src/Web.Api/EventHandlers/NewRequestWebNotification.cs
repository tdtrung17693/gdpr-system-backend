using System;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Web.Api.Core.Domain.Event;
using Web.Api.Core.Dto;
using Web.Api.Core.Dto.Requests;
using Web.Api.Core.Interfaces.Gateways.Repositories;
using Web.Api.Core.Interfaces.Services;
using Web.Api.Core.Interfaces.Services.Event;
using Web.Api.Infrastructure.Data.EntityFramework;

namespace Web.Api.EventHandlers
{
  public class NewRequestWebNotification : IEventHandler<RequestCreated>
  {
    private readonly ApplicationDbContext _context;
    private readonly INotificationRepository _notiRepo;
    private readonly IAuthService _authService;
    public NewRequestWebNotification(ApplicationDbContext context, INotificationRepository notiRepo, IAuthService authService)
    {
      _context = context;
      _notiRepo = notiRepo;
      _authService = authService;
    }

    public async Task HandleAsync(RequestCreated ev)
    {
      var admin = await _context.User.Include(user => user.Role).Where(user => user.Role.Name == "Administrator").ToListAsync();
      await _notiRepo.CreateNewRequestNotification(new Requester()
      {
        FullName = ev.RequesterFullName,
        Username = ev.RequesterUsername,
        UserId = ev.RequesterId
      }, admin, ev.ServerName, ev.ServerId, ev.RequestId);
    }
  }
}
