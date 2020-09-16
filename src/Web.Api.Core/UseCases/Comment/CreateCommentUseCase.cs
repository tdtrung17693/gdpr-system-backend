using System.Threading.Tasks;
using Web.Api.Core.Dto;
using Web.Api.Core.Dto.UseCaseRequests.Comment;
using Web.Api.Core.Dto.UseCaseResponses.Comment;
using Web.Api.Core.Interfaces;
using Web.Api.Core.Interfaces.Gateways.Repositories;
using Web.Api.Core.Interfaces.UseCases.Comment;

namespace Web.Api.Core.UseCases.Comment
{
    public class CreateCommentUseCase : ICreateCommentUseCase
    {
        private readonly ICommentRepository _commentRepository;

        public CreateCommentUseCase(ICommentRepository commentRepository)
        {
            _commentRepository = commentRepository;
        }
        public async Task<bool> Handle(CreateCommentRequest message, IOutputPort<CreateCommentResponse> outputPort)
        {
            var requestId = message.RequestId;
            var parentId = message.ParentId;
            var author = message.Author;
            var content = message.Content;

            var response = await  _commentRepository.CreateCommentOfRequest(
                requestId,
                content,
                author,
                parentId
            );

            if (!response.Success)
            {
                outputPort.Handle(
                    new CreateCommentResponse(response.Errors)
                );
                return false;
            }
            
            outputPort.Handle(new CreateCommentResponse(response.Id, response.CreatedAt));
            return true;
        }
    }
}