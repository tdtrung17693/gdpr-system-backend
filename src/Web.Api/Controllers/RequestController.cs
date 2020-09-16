using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Web.Api.Core.Dto.UseCaseRequests;
using Web.Api.Core.Interfaces.Gateways.Repositories;
using Web.Api.Core.Interfaces.UseCases;
using Web.Api.Core.Interfaces.UseCases.RequestInterface;
using Web.Api.Core.UseCases;
using Web.Api.Models.Request;
using Web.Api.Presenters;
//using Web.Api.Core.Dto.UseCaseRequests;
using System.IO;
using System.Threading;
using Microsoft.AspNetCore.Hosting;
using OfficeOpenXml;
using Web.Api.Infrastructure.Helpers;

namespace Web.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RequestController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IRequestRepository _repository;
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

        public RequestController(IMapper mapper, IRequestRepository repository,
                                ICreateRequestUseCase createRequestUseCase, CreateRequestPresenter createRequestPresenter,
                                IUpdateRequestUseCase updateRequestUseCase, UpdateRequestPresenter updateRequestPresenter,
                                IGetRequestUseCase getRequestUseCase, GetRequestPresenter getRequestPresenter,
                                IExportUseCase exportUseCase, ExportPresenter exportPresenter,
                                IManageRequestUseCase manageRequestUseCase, ManageRequestPresenter manageRequestPresenter,
                                IGetEachRequestUseCase getEachRequestUseCase, GetEachRequestPresenter getEachRequestPresenter)

        {
            _mapper = mapper;
            _repository = repository;
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

        }

        //CREATE
        [EnableCors("request")]
        [HttpPost("create")]
        public async Task<ActionResult> CreateRequest([FromBody] Models.Request.CreateRequestRequestModel message)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            await _createRequestUseCase.Handle(new CreateRequestRequest(message.CreatedBy, message.Title, message.StartDate, message.EndDate, message.ServerId, message.Description), _createRequestPresenter);
            return _createRequestPresenter.ContentResult;
        }

        [EnableCors("request")]
        [HttpPost("exportRequest")]
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
        [EnableCors("request")]
        [HttpGet]
        public async Task<ActionResult> GetRequestPaging(int _pageNo = Constants.DefaultValues.Paging.PageNo, int _pageSize = Constants.DefaultValues.Paging.PageSize,
                            string keyword = Constants.DefaultValues.keyword, string filterStatus = Constants.DefaultValues.filterStatus/*, 
                            DateTime? fromDateExport = null, DateTime? toDateExport = null*/)
        {
            await _getRequestUseCase.Handle(new GetRequestRequest(_pageNo, _pageSize, keyword, filterStatus, /*fromDateExport, toDateExport,*/ "getAll"), _getRequestPresenter);

            return _getRequestPresenter.ContentResult;
        }

        [EnableCors("request")]
        [HttpGet("request/{requestId}")]
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
        [EnableCors("request")]
        [HttpPut("update/{requestId}")]
        public async Task<ActionResult> UpdateRequest(string requestId, [FromBody] Models.Request.UpdateRequestRequestModel message)
        {
            if (!ModelState.IsValid)
            {
                        return BadRequest(ModelState);
            }
            await _updateRequestUseCase.Handle(new UpdateRequestRequest(new Guid(requestId), message.UpdatedBy,DateTime.UtcNow, message.Title, message.StartDate, message.EndDate, message.ServerId, message.Description, message.RequestStatus, message.Response, message.ApprovedBy), _updateRequestPresenter);
            return _updateRequestPresenter.ContentResult;
        }

        [HttpPut("manage")]
        public async Task<ActionResult> ManageRequest([FromBody] Models.Request.ManageRequestRequestModel manageRequestRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            await _manageRequestUseCase.Handle(new ManageRequestRequest(manageRequestRequest.userId, manageRequestRequest.answer, manageRequestRequest.status, manageRequestRequest.requestId), _manageRequestPresenter);
            return _manageRequestPresenter.ContentResult;
        }
    }
}