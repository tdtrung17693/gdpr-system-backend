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
using BulkRequestRequest = Web.Api.Core.Dto.UseCaseRequests.BulkRequestRequest;
using System.IO;
using System.Threading;
using Microsoft.AspNetCore.Hosting;
using OfficeOpenXml;

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
        private readonly IBulkRequestUseCase _bulkRequestUseCase;
        private readonly CreateRequestPresenter _createRequestPresenter;
        private readonly UpdateRequestPresenter _updateRequestPresenter;
        private readonly BulkRequestPresenter _bulkRequestPresenter;

        public RequestController(IMapper mapper, IRequestRepository repository, ICreateRequestUseCase createRequestUseCase,
            CreateRequestPresenter createRequestPresenter, UpdateRequestPresenter updateRequestPresenter, IUpdateRequestUseCase updateRequestUseCase,
            IBulkRequestUseCase bulkRequestUseCase, BulkRequestPresenter bulkRequestPresenter)
        {
            _mapper = mapper;
            _repository = repository;
            _createRequestUseCase = createRequestUseCase;
            _updateRequestUseCase = updateRequestUseCase;
            _bulkRequestUseCase = bulkRequestUseCase;
            _createRequestPresenter = createRequestPresenter;
            _updateRequestPresenter = updateRequestPresenter;
            _bulkRequestPresenter = bulkRequestPresenter;
        }

        //CREATE
        [EnableCors("request")]
        [HttpPost("create")]
        public async Task<ActionResult> CreateNewRequest([FromBody] RequestRequest request)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            await _createRequestUseCase.Handle(new CreateRequestRequest(request.Id, request.CreatedBy, request.CreatedAt, /*request.UpdatedBy, request.UpdatedAt, request.DeletedBy, request.DeletedAt,*/
                request.Title, request.Description, request.StartDate, request.EndDate, request.ServerId, request.RequestStatus/*, request.Response, request.ApprovedBy*/), _createRequestPresenter);
            return Ok();


        }

        //READ
        [EnableCors("request")]
        [HttpGet]
        public ActionResult<IEnumerable<RequestRequest>> GetRequestList()
        {
            var requestItems = _repository.GetRequestList();
            return Ok(_mapper.Map<IEnumerable<RequestRequest>>(requestItems));
        }



        //UPDATE

        [EnableCors("request")]
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

        [EnableCors("request")]
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

    }
}
