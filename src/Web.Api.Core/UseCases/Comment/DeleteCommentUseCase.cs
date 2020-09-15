using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Web.Api.Core.Dto;
using Web.Api.Core.Dto.UseCaseRequests.Comment;
using Web.Api.Core.Dto.UseCaseResponses.Comment;
using Web.Api.Core.Interfaces;
using Web.Api.Core.Interfaces.Gateways.Repositories;
using Web.Api.Core.Interfaces.UseCases.Comment;

namespace Web.Api.Core.UseCases.Comment
{
    public class DeleteCommentUseCase : IDeleteCommentUseCase
    {
        private readonly ICommentRepository _commentRepository;

        public DeleteCommentUseCase(ICommentRepository commentRepository)
        {
            _commentRepository = commentRepository;
        }
        public async Task<bool> Handle(DeleteCommentRequest message, IOutputPort<DeleteCommentResponse> outputPort)
        {
            var commentId = message.commentId;

            var response = await _commentRepository.DeleteCommentOfRequest(
                commentId
            );

            if (!response.Success)
            {
                outputPort.Handle(
                    new DeleteCommentResponse(response.Errors)
                );
                return false;
            }

            outputPort.Handle(new DeleteCommentResponse());
            return true;
        }
    }
}