using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Web.Api.Core.Domain.Entities;
using Web.Api.Core.Dto.UseCaseRequests;
using Web.Api.Core.Interfaces.Gateways.Repositories;
using Web.Api.Core.Interfaces.UseCases;
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

        public ServerController(IMapper mapper, IServerRepository repository)   
        {   
            _mapper = mapper;
            _repository = repository;
        }

        
        [HttpGet]
        public ActionResult<IEnumerable<ServerRequest>> GetAllCommands()
        {
            var serverItems = _repository.GetAllCommand();
            //return Ok(commandItems);
            return Ok(_mapper.Map<IEnumerable<ServerRequest>>(serverItems));
        }

    }
}
