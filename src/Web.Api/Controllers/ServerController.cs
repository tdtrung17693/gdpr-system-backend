using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Web.Api.Core.Domain.Entities;
using Web.Api.Core.Dto.UseCaseRequests;
using Web.Api.Core.Interfaces.Gateways.Repositories;
using Web.Api.Core.Interfaces.UseCases;
using Web.Api.Core.UseCases;
//using Web.Api.Infrastructure.Data.Entities;
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
        private readonly CreateServerPresenter _createServerPresenter;
        private readonly JsonSerializer _jsonSerializer;

        public ServerController(IMapper mapper, IServerRepository repository, ICreateServerUseCase createServerUseCase, CreateServerPresenter createServerPresenter)   
        {   
            _mapper = mapper;
            _repository = repository;
            _createServerUseCase = createServerUseCase;
            _createServerPresenter = createServerPresenter;
        }

        //CREATE
        [HttpPost("create")]
        public async Task<ActionResult> CreateNewServer([FromBody] ServerRequest server)
        {

            if (!ModelState.IsValid)
            { // re-render the view when validation failed.
                return BadRequest(ModelState);
            }
            //var serverItem = _repository.Create(server);
            //return Ok(commandItems);
            await _createServerUseCase.Handle(new CreateServerRequest(server.Id, server.CreatedBy, server.CreatedAt, server.UpdatedBy, server.UpdatedAt, server.DeletedBy, server.DeletedAt, server.isDeleted, server.Name, server.IpAddress, server.StartDate, server.EndDate), _createServerPresenter);
            return Ok();
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

        //DELETE

    }
}
