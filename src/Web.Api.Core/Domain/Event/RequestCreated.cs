using System;
using System.Collections.Generic;
using System.Text;
using Web.Api.Core.Domain.Entities;
using Web.Api.Core.Interfaces.Services.Event;

namespace Web.Api.Core.Domain.Event
{
  public class RequestCreated : IEvent
  {
    public User CreatedBy { get; }
    public DateTime CreatedAt { get; }
    public Guid ServerId { get; set; }
    public Guid RequestId { get; set; }

    public RequestCreated(Guid requestId, DateTime createdAt, Guid serverId)
    {
      RequestId = requestId;
      CreatedAt = createdAt;
      ServerId = serverId;
    }
  }
}
