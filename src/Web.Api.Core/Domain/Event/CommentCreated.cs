using System;
using Web.Api.Core.Interfaces.Services.Event;

namespace Web.Api.Core.Domain.Event
{
    public class CommentCreated : IEvent
    {
        public Guid Id { get; }
        public string Content { get; }
        public Guid? ParentId { get; }
        public string AuthorFirstName { get; }
        public string AuthorLastName { get; }
        public Guid RequestId { get; }
        public DateTime CreatedAt { get; }

        public CommentCreated(Guid id, Guid requestId, string content, string authorFirstName, string authorLastName, DateTime createdAt,
            Guid? parentId)
        {
            Id = id;
            RequestId = requestId;
            Content = content;
            AuthorFirstName = authorFirstName;
            AuthorLastName = authorLastName;
            CreatedAt = createdAt;
            ParentId = parentId;
        }
    }
}