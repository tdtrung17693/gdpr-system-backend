using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
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
using Web.Api.Core.Interfaces.UseCases.IRequestUseCases;
using Web.Api.Presenters.Request;
using System.Data;

namespace Web.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RequestController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ICommentRepository _commentRepository;
        private readonly IRequestRepository _requestRepository;
        private readonly ICreateCommentUseCase _createCommentUseCase;
        private readonly IDeleteCommentUseCase _deleteCommentUseCase;
        private readonly ICreateRequestUseCase _createRequestUseCase;
        private readonly IUpdateRequestUseCase _updateRequestUseCase;
        private readonly IGetRequestUseCase _getRequestUseCase;
        private readonly IGetEachRequestUseCase _getEachRequestUseCase;
        private readonly IExportUseCase _exportUseCase;
        private readonly IManageRequestUseCase _manageRequestUseCase;
        private readonly IBulkExportUseCase _bulkExportUseCase;
        private readonly CreateRequestPresenter _createRequestPresenter;
        private readonly UpdateRequestPresenter _updateRequestPresenter;
        private readonly GetRequestPresenter _getRequestPresenter;
        private readonly GetEachRequestPresenter _getEachRequestPresenter;
        private readonly ExportPresenter _exportPresenter;
        private readonly ManageRequestPresenter _manageRequestPresenter;
        private readonly BulkExportPresenter _bulkExportPresenter;
        private readonly ResourcePresenter<CreateCommentResponse> _createCommentPresenter;
        private readonly ResourcePresenter<DeleteCommentResponse> _deleteCommentPresenter;

        public RequestController(
            IExportUseCase exportUseCase, ExportPresenter exportPresenter,
            IManageRequestUseCase manageRequestUseCase, ManageRequestPresenter manageRequestPresenter,
            IBulkExportUseCase bulkExportUseCase, BulkExportPresenter bulkExportPresenter,
            IGetEachRequestUseCase getEachRequestUseCase, GetEachRequestPresenter getEachRequestPresenter,
            IAuthService authService,
            ICommentRepository commentRepository,
            IRequestRepository requestRepository,
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
            _requestRepository = requestRepository;
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
            _bulkExportUseCase = bulkExportUseCase;
            _bulkExportPresenter = bulkExportPresenter;
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
                new CreateRequestRequest((Guid)currentUser.Id, message.Title, message.StartDate, message.EndDate,
                    message.ServerId, message.Description), _createRequestPresenter);
            return _createRequestPresenter.ContentResult;
        }

        [HttpPost("exportRequest")]
        [Authorize("CanExportData")]
        public async Task<ActionResult> GetRequestForExport(ExportRequestModel message)
        {
            if (!ModelState.IsValid)
            { // re-render the view when validation failed.
                return BadRequest(ModelState);
            }
            await _exportUseCase.Handle(new ExportRequest(message.fromDate, message.toDate, message.guids), _exportPresenter);
            return _exportPresenter.ContentResult;
        }

        //READ 
        [HttpGet("totalRows")]
        public ActionResult<DataTable> GetTotalRow(string searchKey = "")
        {
            return _requestRepository.getNoRows(searchKey);
        }

        [HttpGet]
        [Authorize("CanViewRequest")]
        public async Task<ActionResult> GetRequestPaging(Guid? uid ,int _pageNo = Constants.DefaultValues.Paging.PageNo,
            int _pageSize = Constants.DefaultValues.Paging.PageSize,
            string keyword = Constants.DefaultValues.keyword,
            string filterStatus = Constants.DefaultValues.filterStatus )
        {
            await _getRequestUseCase.Handle(
                new GetRequestRequest(uid ,_pageNo, _pageSize, keyword, filterStatus, "getAll"), _getRequestPresenter);

            return _getRequestPresenter.ContentResult;
        }

        [HttpGet("{requestId}")]
        [Authorize("CanEditRequest")]
        public async Task<ActionResult> GetEachRequest(string requestId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _getEachRequestUseCase.Handle(new GetEachRequestRequest(requestId), _getEachRequestPresenter);
            return _getEachRequestPresenter.ContentResult;
        }



        //UPDATE
        [HttpPut("update/{requestId}")]
        [Authorize("CanEditRequest")]
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
            
            var result = new Collection<Object>();

            foreach (var comment in comments)
            {
              var fileInstance = comment.Author.UserFileInstance.FirstOrDefault();
              var avatarFile = fileInstance?.FileInstance;
              byte[] bytes = null;
              string content = "";
              
              if (avatarFile != null)
              {
                bytes = System.IO.File.ReadAllBytes(Path.Combine(avatarFile.Path, $"{avatarFile.FileName}.{avatarFile.Extension}"));
                content = Convert.ToBase64String(bytes);
              }
              result.Add(new
              {
                comment.Id,
                comment.Content,
                comment.ParentId,
                comment.RequestId,
                comment.CreatedAt,
                Author = new {comment.Author.FirstName, comment.Author.LastName, Avatar= avatarFile != null ? content : null}
              });
            }
            return JsonSerializer.SerializeObject(result);
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
        [Authorize("CanManageRequest")]
        public async Task<ActionResult> ManageRequest(
            [FromBody] ManageRequestRequestModel manageRequestRequest)
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

        [HttpPost("bulkExport")]
        [Authorize("CanDataExport")]
        public async Task<ActionResult> BulkExportAction(string id, [FromBody] Models.Request.BulkExportRequest message)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            await _bulkExportUseCase.Handle(new Core.Dto.UseCaseRequests.BulkExportRequest(message.IdList), _bulkExportPresenter);
            return _bulkExportPresenter.ContentResult;
        }
    }
}
