using System;
using Web.Api.Core.Dto.UseCaseResponses.Comment;
using Web.Api.Core.Interfaces;

namespace Web.Api.Core.Dto.UseCaseRequests.Comment
{
    public class DeleteCommentRequest : IUseCaseRequest<DeleteCommentResponse>
    {
        public Guid commentId { get; }

        public DeleteCommentRequest(Guid _commentId)
        {
            commentId = _commentId;
        }
    }
}