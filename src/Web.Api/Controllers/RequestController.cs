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
using Web.Api.Core.UseCases;
using Web.Api.Models.Request;
using Web.Api.Presenters;
using BulkRequestRequest = Web.Api.Core.Dto.UseCaseRequests.BulkRequestRequest;

namespace Web.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RequestController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IRequestRepository _repository;
        private readonly ICreateRequestUsecase _createRequestUseCase;
        private readonly IUpdateRequestUseCase _updateRequestUseCase;
        private readonly IBulkRequestUseCase _bulkRequestUseCase;
        private readonly CreateRequestPresenter _createRequestPresenter;
        private readonly UpdateRequestPresenter _updateRequestPresenter;
        private readonly BulkRequestPresenter _bulkRequestPresenter;

        public RequestController(IMapper mapper, IRequestRepository repository, ICreateRequestUseCase createRequestUseCase,
            CreateRequestPresenter createRequestPresenter, UpdateRequestPresenter updateRequestPresenter, IUpdateRequestUseCase updateRequestUseCase,
            IBulkRequestUseCase bulkRequestUseCase, BulkRequestPresenter bulkRequestPresenter)//, ICreateRequestUseCase createRequestUseCase, CreateRequestPresenter createRequestPresenter   
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
        [HttpPost("create")]
        public async Task<ActionResult> CreateNewRequest([FromBody] RequestRequest request)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            await _createRequestUseCase.Handle(new CreateRequestRequest(/**/), _createRequestPresenter);
            return Ok();


        }

        //READ
        [HttpGet]
        public ActionResult<IEnumerable<RequestRequest>> GetAllCommands()
        {
            var requestItems = _repository.GetAllCommand();
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
            await _updateRequestUseCase.Handle(/**/), _updateRequestPresenter);
            return Ok("You hav update an row");
        }

        //Get detail a request
        [HttpGet("/detail/{id}")]
        public ActionResult<RequestRequest> GetRequestDetail(Guid id)
        {
            var requestItem = _repository.GetRequestDetail(id);
            return Ok(_mapper.Map<RequestRequest>(requestItem));
        }


        //Active/Deactive multi request
        [HttpPut("bulkStatus")]
        public async Task<ActionResult> UpdateMultiStatusRequest([FromBody] Models.Request.BulkRequestRequest bulkRequest)
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
            var response = await _bulkRequestUseCase.Handle(new BulkRequestRequest(idList, bulkRequest.status, bulkRequest.updator), _bulkRequestPresenter);
            if (response) return Ok("Done");
            else return Content("Error");

        }

    }
}
