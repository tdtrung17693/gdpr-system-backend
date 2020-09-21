using System;
using System.Collections.Generic;
using Web.Api.Core.Domain.Entities;
using Web.Api.Core.Interfaces.Services.Event;

namespace Web.Api.Core.Domain.Event
{
  public class RequestUpdated : IEvent
  {
    public Dictionary<string, List<string>> UpdatedFields { get; set; }
    public Guid RequestId { get; set; }
    public Guid UpdatedBy { get; set; }
  }
}
