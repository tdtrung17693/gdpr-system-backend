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
  public class LogAcceptedRejectedRequest : IEventHandler<RequestAcceptedRejected>
  {
    private readonly IServiceProvider _provider;
    public LogAcceptedRejectedRequest(IServiceProvider provider)
    {
      _provider = provider;
    }
    public async Task HandleAsync(RequestAcceptedRejected ev)
    {
      using (var scope = _provider.CreateScope())
      using (var ctx = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>())
      using (var logRepo = scope.ServiceProvider.GetRequiredService<ILogRepository>())
      {
        var updator = await ctx.User.Where(user => user.Id == ev.UpdatedBy).ToListAsync();
        await logRepo.LogAcceptRejectRequest(ev.RequestId, updator[0], ev.OldStatus, ev.NewStatus);
      }
    }
  }
}
