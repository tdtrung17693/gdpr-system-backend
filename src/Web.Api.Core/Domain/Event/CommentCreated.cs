using System;
using Web.Api.Core.Interfaces.Services.Event;

namespace Web.Api.Core.Domain.Event
{
  public class CommentCreated : IEvent
  {
    public Guid Id { get; set; }
    public string Content { get; set; }
    public Guid? ParentId { get; set; }
    public string AuthorFirstName { get; set; }
    public string AuthorLastName { get; set; }
    public string? AuthorAvatar { get; set; }
    public Guid RequestId { get; set; }
    public DateTime CreatedAt { get; set; }

    public CommentCreated(Guid id, Guid requestId, string content, string authorFirstName, string authorLastName, string authorAvatar,
      DateTime createdAt,
      Guid? parentId)
    {
      Id = id;
      RequestId = requestId;
      Content = content;
      AuthorFirstName = authorFirstName;
      AuthorLastName = authorLastName;
      AuthorAvatar = authorAvatar;
      CreatedAt = createdAt;
      ParentId = parentId;
    }
  }
}
