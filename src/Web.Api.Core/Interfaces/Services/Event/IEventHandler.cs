using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Web.Api.Core.Interfaces.Services.Event
{
  public interface IEventHandler<TEvent> where TEvent : IEvent
  {
    Task HandleAsync(TEvent ev);
  }
}
