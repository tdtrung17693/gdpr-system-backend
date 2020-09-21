using System;
using Web.Api.Core.Dto.UseCaseResponses.Comment;
using Web.Api.Core.Interfaces;

namespace Web.Api.Core.Dto.UseCaseRequests.Comment
{
    public class CreateCommentRequest : IUseCaseRequest<CreateCommentResponse>
    {
        public Guid RequestId { get; }
        public string Content { get; }
        public Domain.Entities.User Author { get; }
        public Guid? ParentId { get; }

        public CreateCommentRequest(Guid requestId, string content, Domain.Entities.User author, Guid? parentId)
        {
            RequestId = requestId;
            Content = content;
            Author = author;
            ParentId = parentId;
        }
    }
}