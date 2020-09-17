using System;
using Web.Api.Core.Interfaces.Services.Event;

namespace Web.Api.Core.Domain.Event
{
    public class CommentDeleted : IEvent
    {
        public Guid Id { get; }

        public Guid RequestId { get;  }

        public CommentDeleted(Guid id,  Guid requestId)
        {
            Id = id;
            RequestId = requestId;
        }
    }
}