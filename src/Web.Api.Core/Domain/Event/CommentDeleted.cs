using System;
using Web.Api.Core.Interfaces.Services.Event;

namespace Web.Api.Core.Domain.Event
{
    public class CommentDeleted : IEvent
    {
        public Guid Id { get; }


        public CommentDeleted(Guid id)
        {
            Id = id;
        }
    }
}