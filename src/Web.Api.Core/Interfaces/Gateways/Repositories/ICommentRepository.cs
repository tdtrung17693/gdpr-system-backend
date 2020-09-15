using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Web.Api.Core.Domain.Entities;
using Web.Api.Core.Dto.UseCaseResponses.Comment;

namespace Web.Api.Core.Interfaces.Gateways.Repositories
{
    public interface ICommentRepository
    {
        Task<IEnumerable<Comment>> FindCommentsOfRequest(Guid requestId, string order);
        Task<CreateCommentResponse> CreateCommentOfRequest(Guid requestId, string content, User author, Guid? parentId);
        Task<DeleteCommentResponse> DeleteCommentOfRequest(Guid commentId, Guid requestId);
    }
}