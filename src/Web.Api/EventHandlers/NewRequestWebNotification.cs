using System;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Web.Api.Core.Domain.Event;
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
      var server = _context.Server.Where(server => server.Id == ev.ServerId).FirstOrDefault();
      var user = _authService.GetCurrentUser();
      var admin = await _context.User.Include(user => user.Role).Where(user => user.Role.Name == "Administrator").ToListAsync();
      if (server != null)
      {
        await _notiRepo.CreateNewRequestNotification(user, admin, server.Name, (Guid) server.Id, ev.RequestId);
      }
    }
  }
}
