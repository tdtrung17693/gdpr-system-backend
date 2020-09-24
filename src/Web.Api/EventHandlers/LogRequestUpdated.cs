using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Web.Api.Core.Domain.Event;
using Web.Api.Core.Interfaces.Gateways.Repositories;
using Web.Api.Core.Interfaces.Services.Event;
using Web.Api.Infrastructure.Data.EntityFramework;

namespace Web.Api.EventHandlers
{
  public class LogRequestUpdated : IEventHandler<RequestUpdated>
  {
    private readonly IServiceProvider _provider;
    public LogRequestUpdated(IServiceProvider provider)
    {
      _provider = provider;
    }

    public async Task HandleAsync(RequestUpdated ev)
    {

      using (var scope = _provider.CreateScope())
      using (var ctx = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>())
      using (var logRepo = scope.ServiceProvider.GetRequiredService<ILogRepository>())
      {
        var updator = await ctx.User.Where(user => user.Id == ev.UpdatedBy).ToListAsync();
        await logRepo.LogUpdateRequest(ev.RequestId, ev.UpdatedFields, updator[0]);
      }
    }
  }
}
