using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
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
        private readonly IDeleteCommentUseCase _deleteCommentUseCase;
        private readonly ICreateRequestUseCase _createRequestUseCase;
        private readonly IUpdateRequestUseCase _updateRequestUseCase;
        private readonly IGetRequestUseCase _getRequestUseCase;
        private readonly IGetEachRequestUseCase _getEachRequestUseCase;
        private readonly IExportUseCase _exportUseCase;
        private readonly IManageRequestUseCase _manageRequestUseCase;
        private readonly CreateRequestPresenter _createRequestPresenter;
        private readonly UpdateRequestPresenter _updateRequestPresenter;
        private readonly GetRequestPresenter _getRequestPresenter;
        private readonly GetEachRequestPresenter _getEachRequestPresenter;
        private readonly ExportPresenter _exportPresenter;
        private readonly ManageRequestPresenter _manageRequestPresenter;
        private readonly ResourcePresenter<CreateCommentResponse> _createCommentPresenter;
        private readonly ResourcePresenter<DeleteCommentResponse> _deleteCommentPresenter;

        public RequestController(
            IExportUseCase exportUseCase, ExportPresenter exportPresenter,
            IManageRequestUseCase manageRequestUseCase, ManageRequestPresenter manageRequestPresenter,
            IGetEachRequestUseCase getEachRequestUseCase, GetEachRequestPresenter getEachRequestPresenter,
            IAuthService authService,
            ICommentRepository commentRepository,
            ICreateCommentUseCase createCommentUseCase,
            ICreateRequestUseCase createRequestUseCase,
            CreateRequestPresenter createRequestPresenter,
            UpdateRequestPresenter updateRequestPresenter,
            IUpdateRequestUseCase updateRequestUseCase,
            ResourcePresenter<DeleteCommentResponse> deleteCommentPresenter,
            IDeleteCommentUseCase deleteCommentUseCase,
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
            _getEachRequestUseCase = getEachRequestUseCase;
            _getEachRequestPresenter = getEachRequestPresenter;
            _exportUseCase = exportUseCase;
            _exportPresenter = exportPresenter;
            _manageRequestUseCase = manageRequestUseCase;
            _manageRequestPresenter = manageRequestPresenter;

            _createCommentPresenter = createCommentPresenter;
            _deleteCommentPresenter = deleteCommentPresenter;
            _deleteCommentUseCase = deleteCommentUseCase;
        }

        //CREATE
        [HttpPost("create")]
        [Authorize("CanCreateRequest")]
        public async Task<ActionResult> CreateRequest([FromBody] CreateRequestRequestModel message)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var currentUser = _authService.GetCurrentUser();
            await _createRequestUseCase.Handle(
                new CreateRequestRequest((Guid) currentUser.Id, message.Title, message.StartDate, message.EndDate,
                    message.ServerId, message.Description), _createRequestPresenter);
            return _createRequestPresenter.ContentResult;
        }

        [HttpPost("exportRequest")]
        [Authorize("CanExportData")]
        public async Task<ActionResult> GetRequestForExport(ExportRequestModel message)
        {
            if (!ModelState.IsValid)
            {
                // re-render the view when validation failed.
                return BadRequest(ModelState);
            }

            await _exportUseCase.Handle(new ExportRequest(message.fromDate, message.toDate, message.guids),
                _exportPresenter);
            return _exportPresenter.ContentResult;
        }

        //READ 
        [HttpGet]
        public async Task<ActionResult> GetRequestPaging(int _pageNo = Constants.DefaultValues.Paging.PageNo,
            int _pageSize = Constants.DefaultValues.Paging.PageSize,
            string keyword = Constants.DefaultValues.keyword,
            string filterStatus = Constants.DefaultValues.filterStatus /*, 
                            DateTime? fromDateExport = null, DateTime? toDateExport = null*/)
        {
            await _getRequestUseCase.Handle(
                new GetRequestRequest(_pageNo, _pageSize, keyword, filterStatus, /*fromDateExport, toDateExport,*/
                    "getAll"), _getRequestPresenter);

            return _getRequestPresenter.ContentResult;
        }

        [HttpGet("{requestId}")]
        public async Task<ActionResult> GetEachRequest(string requestId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _getEachRequestUseCase.Handle(new GetEachRequestRequest(requestId), _getEachRequestPresenter);
            return _getEachRequestPresenter.ContentResult;
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


        [HttpGet("{id}/comments/{order}")]
        [Authorize("CanViewRequest")]
        public async Task<string> GetCommentsOfRequest(Guid id, string order)
        {
            var comments = await _commentRepository.FindCommentsOfRequest(id, order);
            return JsonSerializer.SerializeObject(comments.Select(c =>
            {
                return new
                {
                    c.Id,
                    c.Content,
                    c.ParentId,
                    c.RequestId,
                    c.CreatedAt,
                    Author = new {FirstName = c.Author.FirstName, LastName = c.Author.LastName}
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

        [HttpDelete("{requestId}/comments/{id}")]
        [Authorize("CanEditRequest")]
        public async Task<IActionResult> DeleteCommentOfRequest(string requestId, string id)
        {
            _deleteCommentPresenter.HandleResource = r =>
            {
                return r.Success
                    ? JsonSerializer.SerializeObject(new {r.Id})
                    : JsonSerializer.SerializeObject(new {r.Errors});
            };
            var response = await _deleteCommentUseCase.Handle(
                new Core.Dto.UseCaseRequests.Comment.DeleteCommentRequest(Guid.Parse(id), Guid.Parse(requestId)),
                _deleteCommentPresenter);

            return _deleteCommentPresenter.ContentResult;
        }


        [HttpPut("manage")]
        public async Task<ActionResult> ManageRequest(
            [FromBody] Models.Request.ManageRequestRequestModel manageRequestRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _manageRequestUseCase.Handle(
                new ManageRequestRequest(manageRequestRequest.userId, manageRequestRequest.answer,
                    manageRequestRequest.status, manageRequestRequest.requestId), _manageRequestPresenter);
            return _manageRequestPresenter.ContentResult;
        }
    }
}