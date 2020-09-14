using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Web.Api.Core.Domain.Entities;
using Web.Api.Core.Dto.GatewayResponses.Repositories;

namespace Web.Api.Core.Interfaces.Gateways.Repositories
{
    public interface ICommentRepository
    {
        Task<IEnumerable<Comment>> FindCommentsOfRequest(Guid requestId);
        Task<CreateCommentResponse> CreateCommentOfRequest(Guid requestId, string content, User author, Guid? parentId);
    }
}