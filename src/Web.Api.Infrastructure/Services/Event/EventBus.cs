using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hangfire;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Web.Api.Core.Interfaces.Services.Event;

namespace Web.Api.Infrastructure.Services.Event
{
  public class EventBus : IDomainEventBus
  {
    private readonly ConcurrentDictionary<Type, List<Type>> _eventHandlers;
    private readonly IServiceProvider _provider;
    public EventBus(IServiceProvider serviceProvider)
    {
      _eventHandlers = new ConcurrentDictionary<Type, List<Type>>();

      _provider = serviceProvider;
    }
    public async Task Trigger<TEvent>(TEvent ev) where TEvent : IEvent
    {
      var eventType = typeof(TEvent);
      if (_eventHandlers.ContainsKey(eventType))
      {
        foreach (var handler in _eventHandlers.First(pair => pair.Key == eventType).Value)
        {
          BackgroundJob.Enqueue(() =>
            ((IEventHandler<TEvent>) _provider.GetRequiredService(handler)).HandleAsync(ev)
          );
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
