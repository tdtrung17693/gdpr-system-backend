using System;
using Web.Api.Core.Interfaces.Services.Event;

namespace Web.Api.Core.Domain.Event
{
  public class CommentDeleted : IEvent
  {
    public Guid Id { get; set; }

    public Guid RequestId { get; set; }

    public CommentDeleted(Guid id, Guid requestId)
    {
      Id = id;
      RequestId = requestId;
    }
  }
}
