﻿using Web.Api.Core.Dto.UseCaseRequests.Comment;
using Web.Api.Core.Dto.UseCaseResponses.Comment;

namespace Web.Api.Core.Interfaces.UseCases.Comment
{
    public interface IDeleteCommentUseCase : IUseCaseRequestHandler<DeleteCommentRequest, DeleteCommentResponse>
    {

    }
}