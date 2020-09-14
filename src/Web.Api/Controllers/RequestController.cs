using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Web.Api.Core.Dto.UseCaseRequests;
using Web.Api.Core.Interfaces.Gateways.Repositories;
using Web.Api.Core.Interfaces.UseCases;
using Web.Api.Core.Interfaces.UseCases.RequestInterface;
using Web.Api.Models.Request;
using Web.Api.Presenters;
using BulkRequestRequest = Web.Api.Core.Dto.UseCaseRequests.BulkRequestRequest;
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
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;
        private readonly IRequestRepository _repository;
        private readonly ICommentRepository _commentRepository;
        private readonly ICreateCommentUseCase _createCommentUseCase;
        private readonly IDeleteCommentUseCase _deleteCommentUseCase;
        private readonly ICreateRequestUseCase _createRequestUseCase;
        private readonly IUpdateRequestUseCase _updateRequestUseCase;
        private readonly IBulkRequestUseCase _bulkRequestUseCase;
        private readonly CreateRequestPresenter _createRequestPresenter;
        private readonly UpdateRequestPresenter _updateRequestPresenter;
        private readonly BulkRequestPresenter _bulkRequestPresenter;
        private readonly ResourcePresenter<CreateCommentResponse> _createCommentPresenter;
        private readonly ResourcePresenter<DeleteCommentResponse> _deleteCommentPresenter;

        public RequestController(
            IMapper mapper,
            IAuthService authService,
            IRequestRepository repository,
            ICommentRepository commentRepository,
            ICreateCommentUseCase createCommentUseCase,
            ICreateRequestUseCase createRequestUseCase,
            CreateRequestPresenter createRequestPresenter,
            UpdateRequestPresenter updateRequestPresenter,
            IUpdateRequestUseCase updateRequestUseCase,
            IBulkRequestUseCase bulkRequestUseCase,
            BulkRequestPresenter bulkRequestPresenter,
            ResourcePresenter<CreateCommentResponse> createCommentPresenter,
            ResourcePresenter<DeleteCommentResponse> deleteCommentPresenter,
            IDeleteCommentUseCase deleteCommentUseCase)
        {
            _mapper = mapper;
            _repository = repository;
            _authService = authService;
            _commentRepository = commentRepository;
            _createCommentUseCase = createCommentUseCase;
            _createRequestUseCase = createRequestUseCase;
            _updateRequestUseCase = updateRequestUseCase;
            _bulkRequestUseCase = bulkRequestUseCase;
            _createRequestPresenter = createRequestPresenter;
            _updateRequestPresenter = updateRequestPresenter;
            _bulkRequestPresenter = bulkRequestPresenter;
            _createCommentPresenter = createCommentPresenter;
            _deleteCommentPresenter = deleteCommentPresenter;
            _deleteCommentUseCase = deleteCommentUseCase;
        }

        //CREATE
        [HttpPost("create")]
        public async Task<ActionResult> CreateNewRequest([FromBody] RequestRequest request)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            await _createRequestUseCase.Handle(new CreateRequestRequest(request.Id, request.CreatedBy, request.CreatedAt, request.UpdatedBy, request.UpdatedAt, request.DeletedBy, request.DeletedAt,
                request.Title, request.Description, request.StartDate, request.EndDate, request.ServerId, request.RequestStatus, request.Response, request.ApprovedBy), _createRequestPresenter);
            return Ok();


        }

        //READ
        [HttpGet]
        public ActionResult<IEnumerable<RequestRequest>> GetRequestList()
        {
            var requestItems = _repository.GetRequestList();
            return Ok(_mapper.Map<IEnumerable<RequestRequest>>(requestItems));
        }



        //UPDATE

        [HttpPut]
        public async Task<ActionResult> UpdateRequest([FromBody] RequestRequest request)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            await _updateRequestUseCase.Handle(new UpdateRequestRequest(request.Id, request.CreatedBy, request.CreatedAt, request.UpdatedBy, request.UpdatedAt, request.DeletedBy, request.DeletedAt,
                request.Title, request.Description, request.StartDate, request.EndDate, request.ServerId, request.RequestStatus, request.Response, request.ApprovedBy), _updateRequestPresenter);
            return Ok();
        }

        //Active/Deactive multi request

        [HttpPut("bulkStatus")]
        public async Task<ActionResult> UpdateMultiStatusRequest([FromBody] Models.Request.BulkRequestsRequest bulkRequest)
        //IEnumerable<Guid> requestIdList,bool status, Guid updator
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //create table type
            DataTable idList = new DataTable();
            idList.Columns.Add("Id", typeof(Guid));
            foreach (Guid id in bulkRequest.requestIdList)
            {
                idList.Rows.Add(id);
            }



            //var response = await _repository.UpdateMutilRequestStatus(idList, bulkRequest.status, bulkRequest.updator);
            var response = await _bulkRequestUseCase.Handle(new BulkRequestRequest(idList, bulkRequest.requestStatus, bulkRequest.updator), _bulkRequestPresenter);
            if (response) return Ok("Done");
            else return Content("Error");

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
                    Author = new { FirstName = c.Author.FirstName, LastName = c.Author.LastName }
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
                    ? JsonSerializer.SerializeObject(new { r.Id, r.CreatedAt })
                    : JsonSerializer.SerializeObject(new { r.Errors });
            };
            var currentUser = _authService.GetCurrentUser();

            var response = await _createCommentUseCase.Handle(new Core.Dto.UseCaseRequests.Comment.CreateCommentRequest(
                id, request.Content, currentUser, request.ParentId
            ), _createCommentPresenter);

            return _createCommentPresenter.ContentResult;
        }

        [HttpDelete("{id}/comments")]
        [Authorize("CanEditRequest")]
        public async Task<IActionResult> DeleteCommentOfRequest(Guid id, [FromBody] DeleteCommentRequest request)
        {
            _deleteCommentPresenter.HandleResource = r =>
            {
                return r.Success
                   ? JsonSerializer.SerializeObject(new { r.Id })
                   : JsonSerializer.SerializeObject(new { r.Errors });
            };
            var response = await _deleteCommentUseCase.Handle(new Core.Dto.UseCaseRequests.Comment.DeleteCommentRequest(request.CommentId),
                _deleteCommentPresenter);

            return _deleteCommentPresenter.ContentResult;
        }

    }
}
