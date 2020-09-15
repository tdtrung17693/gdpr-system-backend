using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Web.Api.Core.Dto.UseCaseRequests;
using Web.Api.Core.Interfaces.Gateways.Repositories;
using Web.Api.Core.Interfaces.UseCases;
using Web.Api.Models.Request;
using Web.Api.Presenters;
using Web.Api.Infrastructure.Helpers;
using Microsoft.AspNetCore.Authorization;
using Web.Api.Core.Dto.UseCaseResponses.Comment;
using Web.Api.Core.Interfaces.Services;
using Web.Api.Core.Interfaces.UseCases.Comment;
using Web.Api.Models.Request.Comment;
using Web.Api.Serialization;

namespace Web.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RequestController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ICommentRepository _commentRepository;
        private readonly ICreateCommentUseCase _createCommentUseCase;
        private readonly ICreateRequestUseCase _createRequestUseCase;
        private readonly IUpdateRequestUseCase _updateRequestUseCase;
        private readonly IGetRequestUseCase _getRequestUseCase;
        private readonly CreateRequestPresenter _createRequestPresenter;
        private readonly UpdateRequestPresenter _updateRequestPresenter;
        private readonly GetRequestPresenter _getRequestPresenter;
        private readonly ResourcePresenter<CreateCommentResponse> _createCommentPresenter;

        public RequestController(
            IAuthService authService,
            ICommentRepository commentRepository,
            ICreateCommentUseCase createCommentUseCase,
            ICreateRequestUseCase createRequestUseCase,
            CreateRequestPresenter createRequestPresenter,
            UpdateRequestPresenter updateRequestPresenter,
            IUpdateRequestUseCase updateRequestUseCase,
            IGetRequestUseCase getRequestUseCase,
            GetRequestPresenter getRequestPresenter,
            ResourcePresenter<CreateCommentResponse> createCommentPresenter)
        {
            _authService = authService;
            _commentRepository = commentRepository;
            _createCommentUseCase = createCommentUseCase;
            _createRequestUseCase = createRequestUseCase;
            _createRequestPresenter = createRequestPresenter;
            _updateRequestUseCase = updateRequestUseCase;
            _updateRequestPresenter = updateRequestPresenter;
            _getRequestUseCase = getRequestUseCase;
            _getRequestPresenter = getRequestPresenter;
            _createCommentPresenter = createCommentPresenter;
        }

        //CREATE
        [HttpPost("create")]
        public async Task<ActionResult> CreateRequest([FromBody] CreateRequestRequestModel message)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _createRequestUseCase.Handle(
                new CreateRequestRequest(message.CreatedBy, message.Title, message.StartDate, message.EndDate,
                    message.ServerId, message.Description), _createRequestPresenter);
            return _createRequestPresenter.ContentResult;
        }

        //READ
        [HttpGet]
        public async Task<ActionResult> GetRequestPaging(int _pageNo = Constants.DefaultValues.Paging.PageNo,
            int _pageSize = Constants.DefaultValues.Paging.PageSize)
        {
            await _getRequestUseCase.Handle(new GetRequestRequest(_pageNo, _pageSize, "getAll"), _getRequestPresenter);

            return _getRequestPresenter.ContentResult;
        }

        //[HttpGet("search/{keyword}")]
        //public ActionResult<IEnumerable<RequestJoined>> GetRequestFilter(string keyword, int pageNo = Constants.DefaultValues.Paging.PageNo, int pageSize = Constants.DefaultValues.Paging.PageSize)
        //{
        //    var requestItems = _repository.GetRequestFilter(keyword, pageNo, pageSize);
        //    return Ok(_mapper.Map<IEnumerable<RequestJoined>>(requestItems));
        //}


        //UPDATE
        [HttpPut("update/{requestId}")]
        public async Task<ActionResult> UpdateRequest(string requestId, [FromBody] UpdateRequestRequestModel message)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _updateRequestUseCase.Handle(
                new UpdateRequestRequest(new Guid(requestId), message.UpdatedBy, DateTime.UtcNow, message.Title,
                    message.StartDate, message.EndDate, message.ServerId, message.Description, message.RequestStatus,
                    message.Response, message.ApprovedBy), _updateRequestPresenter);
            return _updateRequestPresenter.ContentResult;
        }

        [HttpGet("{id}/comments")]
        [Authorize("CanViewRequest")]
        public async Task<string> GetCommentsOfRequest(Guid id)
        {
            var comments = await _commentRepository.FindCommentsOfRequest(id);
            return JsonSerializer.SerializeObject(comments.Select(c =>
            {
                return new
                {
                    c.Id,
                    c.Content,
                    c.ParentId,
                    c.RequestId,
                    c.CreatedAt,
                    Author = new {c.Author.FirstName, c.Author.LastName}
                };
            }));
        }

        [HttpPost("{id}/comments")]
        [Authorize("CanEditRequest")]
        public async Task<IActionResult> CreateCommentForRequest(Guid id, [FromBody] CreateCommentRequest request)
        {
            _createCommentPresenter.HandleResource = r =>
            {
                return r.Success
                    ? JsonSerializer.SerializeObject(new {r.Id, r.CreatedAt})
                    : JsonSerializer.SerializeObject(new {r.Errors});
            };
            var currentUser = _authService.GetCurrentUser();

            await _createCommentUseCase.Handle(new Core.Dto.UseCaseRequests.Comment.CreateCommentRequest(
                id, request.Content, currentUser, request.ParentId
            ), _createCommentPresenter);

            return _createCommentPresenter.ContentResult;
        }
    }
}