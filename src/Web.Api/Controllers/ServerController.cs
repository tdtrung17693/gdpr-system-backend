using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Web.Api.Core.Dto.UseCaseRequests;
using Web.Api.Core.Dto.UseCaseRequests.ServerUserCaseRequest;
using Web.Api.Core.Interfaces.Gateways.Repositories;
using Web.Api.Core.Interfaces.UseCases.ServerInterface;
using Web.Api.Core.UseCases;
using Web.Api.Models.Request;
using Web.Api.Presenters;
using BulkServerRequest = Web.Api.Core.Dto.UseCaseRequests.ServerUserCaseRequest.BulkServerRequest;

namespace Web.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServerController : ControllerBase
    {   
        private readonly IMapper _mapper;
        private readonly IServerRepository _repository;
        private readonly ICreateServerUseCase _createServerUseCase;
        private readonly IUpdateServerUseCase _updateServerUseCase;
        private readonly IBulkServerUseCase _bulkServerUseCase;
        private readonly CreateServerPresenter _createServerPresenter;
        private readonly UpdateServerPresenter _updateServerPresenter;
        private readonly BulkServerPresenter _bulkServerPresenter;

        public ServerController(IMapper mapper, IServerRepository repository, ICreateServerUseCase createServerUseCase, 
            CreateServerPresenter createServerPresenter, UpdateServerPresenter updateServerPresenter ,IUpdateServerUseCase updateServerUseCase,
            IBulkServerUseCase bulkServerUseCase, BulkServerPresenter bulkServerPresenter)//, ICreateServerUseCase createServerUseCase, CreateServerPresenter createServerPresenter   
        {   
            _mapper = mapper;
            _repository = repository;
            _createServerUseCase = createServerUseCase;
            _updateServerUseCase = updateServerUseCase;
            _bulkServerUseCase = bulkServerUseCase;
            _createServerPresenter = createServerPresenter;
            _updateServerPresenter = updateServerPresenter;
            _bulkServerPresenter = bulkServerPresenter;
        }

        //CREATE
        [EnableCors("server")]
        [HttpPost("create")]
        public async Task<ActionResult> CreateNewServer([FromBody] ServerRequest server)   
        {

            if (!ModelState.IsValid)
            { 
                return BadRequest(ModelState);
            }
            await _createServerUseCase.Handle(new CreateServerRequest(server.Id, server.CreatedAt, server.CreatedBy, server.DeletedAt, server.DeletedBy, server.EndDate,
            server.IpAddress, server.IsDeleted, server.Name,
             server.StartDate, server.Status, server.UpdatedAt, server.UpdatedBy), _createServerPresenter);
            return Ok("You hav add an row");
           
           
        }

        //READ
        [HttpGet]
        public ActionResult<IEnumerable<ServerRequest>> GetAllCommands()
        {
            var serverItems = _repository.GetAllCommand();
            return Ok(_mapper.Map<IEnumerable<ServerRequest>>(serverItems));
        }

        //UPDATE
        [EnableCors("server")]
        [HttpPut]
        public async Task<ActionResult> UpdateServer([FromBody] ServerRequest server)
        {

            if (!ModelState.IsValid) 
            { 
                return BadRequest(ModelState);
            }
            await _updateServerUseCase.Handle(new UpdateServerRequest(server.Id, server.CreatedAt, server.CreatedBy, server.DeletedAt, server.DeletedBy, server.EndDate,
            server.IpAddress, server.IsDeleted, server.Name,
             server.StartDate, server.Status, server.UpdatedAt, server.UpdatedBy) , _updateServerPresenter);
            return Ok("You hav update an row");
        }

        //Get detail a server
        [HttpGet("/detail/{id}")]
        public ActionResult<ServerRequest> GetServerDetail(Guid id)
        {
            var serverItem = _repository.GetServerDetail(id);
            return Ok(_mapper.Map<ServerRequest>(serverItem));
        }


        //Active/Deactive multi server
        [HttpPut("bulkStatus")]
        public async Task<ActionResult> UpdateMultiStatusServer([FromBody] Models.Request.BulkServerRequest bulkServer )//IEnumerable<Guid> serverIdList,bool status, Guid updator
        {

            if (!ModelState.IsValid)
            { 
                return BadRequest(ModelState);
            }

            //create table type
            DataTable idList = new DataTable();
            idList.Columns.Add("Id", typeof(Guid));
            foreach(Guid id in bulkServer.serverIdList)
            {
                idList.Rows.Add(id);
            }



            //var response = await _repository.UpdateMutilServerStatus(idList, bulkServer.status, bulkServer.updator);
            var response = await _bulkServerUseCase.Handle( new BulkServerRequest(idList, bulkServer.status, bulkServer.updator) , _bulkServerPresenter);
            if (response) return Ok("Done");
            else return Content("Erorr");

        }

    }
}
