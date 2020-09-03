using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Web.Api.Core.Dto.UseCaseRequests;
using Web.Api.Core.Dto.UseCaseRequests.ServerUserCaseRequest;
using Web.Api.Core.Interfaces.Gateways.Repositories;
using Web.Api.Core.Interfaces.UseCases.ServerInterface;
using Web.Api.Models.Request;
using Web.Api.Presenters;

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
        private readonly CreateServerPresenter _createServerPresenter;
        private readonly UpdateServerPresenter _updateServerPresenter;
        //private readonly JsonSerializer _jsonSerializer;

        public ServerController(IMapper mapper, IServerRepository repository, ICreateServerUseCase createServerUseCase, CreateServerPresenter createServerPresenter, UpdateServerPresenter updateServerPresenter ,IUpdateServerUseCase updateServerUseCase)//, ICreateServerUseCase createServerUseCase, CreateServerPresenter createServerPresenter   
        {   
            _mapper = mapper;
            _repository = repository;
            _createServerUseCase = createServerUseCase;
            _updateServerUseCase = updateServerUseCase;
            _createServerPresenter = createServerPresenter;
            _updateServerPresenter = updateServerPresenter;
        }

        //CREATE
        [EnableCors("server")]
        [HttpPost("create")]
        public async Task<ActionResult> CreateNewServer([FromBody] ServerRequest server)   
        {

            if (!ModelState.IsValid)
            { // re-render the view when validation failed.
                return BadRequest(ModelState);
            }
            //var serverItem = _repository.Create(server);
            //return Ok(commandItems);
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
            //return Ok(commandItems);
            return Ok(_mapper.Map<IEnumerable<ServerRequest>>(serverItems));
        }

        //UPDATE
        [EnableCors("server")]
        [HttpPut("update")]
        public async Task<ActionResult> UpdateServer([FromBody] ServerRequest server)
        {

            if (!ModelState.IsValid)
            { // re-render the view when validation failed.
                return BadRequest(ModelState);
            }
            //var serverItem = _repository.Create(server);
            //return Ok(commandItems);
            await _updateServerUseCase.Handle(new UpdateServerRequest("ggg","ggg") , _updateServerPresenter);
            return Ok("You hav update an row");


        }


        //DELETE

    }
}
