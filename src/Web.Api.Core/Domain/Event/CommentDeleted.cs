using System;
using Web.Api.Core.Interfaces.Services.Event;

namespace Web.Api.Core.Domain.Event
{
    public class CommentDeleted : IEvent
    {
        public Guid Id { get; }
        public string Content { get; }
        public Guid? ParentId { get; }
        public Guid RequestId { get; }

        public CommentDeleted(Guid id, Guid requestId, string content, 
            Guid? parentId)
        {
            Id = id;
            RequestId = requestId;
            Content = content;
            ParentId = parentId;
        }
    }
}