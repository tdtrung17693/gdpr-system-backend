using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Web.Api.Core.Interfaces.Services.Event;

namespace Web.Api.Infrastructure.Services.Event
{
  public class EventBus : IDomainEventBus
  {
    private readonly ConcurrentDictionary<Type, List<Type>> _eventHandlers;
    private readonly IHttpContextAccessor _context;
    public EventBus(IHttpContextAccessor context)
    {
      _eventHandlers = new ConcurrentDictionary<Type, List<Type>>();
      _context = context;
    }
    public async Task Trigger<TEvent>(TEvent ev) where TEvent : IEvent
    {
      var eventType = typeof(TEvent);
      if (_eventHandlers.ContainsKey(eventType))
      {
        foreach (var handler in _eventHandlers.First(pair => pair.Key == eventType).Value)
        {
          await ((IEventHandler<TEvent>)_context.HttpContext.RequestServices.GetService(handler)).HandleAsync(ev);
        }
      }
    }

    public  void AddEventHandler<TEvent, TEventHandler>()
      where TEvent : IEvent
      where TEventHandler : IEventHandler<TEvent>
    {
      _eventHandlers.GetOrAdd(typeof(TEvent), type => new List<Type>()).Add(typeof(TEventHandler));
    }
  }
}
