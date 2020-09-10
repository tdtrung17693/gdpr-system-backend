using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Web.Api.Core.Interfaces.Services.Event
{
  public interface IDomainEventBus
  {
    Task Trigger<TEvent>(TEvent ev) where TEvent : IEvent;
    void AddEventHandler<TEvent, TEventHandler>() where TEvent : IEvent where TEventHandler : IEventHandler<TEvent>;
  }
}
