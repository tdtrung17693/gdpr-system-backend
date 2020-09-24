using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Api.Core.Domain.Entities;
using Web.Api.Core.Domain.Event;
using Web.Api.Core.Interfaces.Gateways.Repositories;
using Web.Api.Core.Interfaces.Services.Event;
using Web.Api.Infrastructure.Data.EntityFramework;

namespace Web.Api.EventHandlers
{
  public class LogNewRequest : IEventHandler<RequestCreated>
  {
    private readonly IServiceProvider _provider;
    public LogNewRequest(IServiceProvider provider)
    {
      _provider = provider;
    }
    public async Task HandleAsync(RequestCreated ev)
    {
      using (var scope = _provider.CreateScope())
      using (var ctx = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>())
      using (var logRepo = scope.ServiceProvider.GetRequiredService<ILogRepository>())
      {
        var creator = await ctx.User.Where(user => user.Id == ev.RequesterId).ToListAsync();
        await logRepo.LogNewRequest(ev.RequestId, creator[0]);
      }
    }
  }
}
