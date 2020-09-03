using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Web.Api.Core.Domain.Entities;
using Web.Api.Core.Interfaces.Gateways.Repositories;
using Web.Api.Core.Interfaces.UseCases;
using Web.Api.Models.Request;
using Web.Api.Presenters;

namespace Web.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RequestController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IRequestRepository _repository;

        public RequestController(IMapper mapper, IRequestRepository repository)
        {
            _mapper = mapper;
            _repository = repository;
        }


        [HttpGet]
        public ActionResult<IEnumerable<RequestRequest>> GetRequestList()
        {
            var RequestItems = _repository.GetRequestList();
            return Ok(_mapper.Map<IEnumerable<RequestRequest>>(RequestItems));
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] Models.Request.RequestRequest request)
        {
            if (!ModelState.IsValid)
            { // re-render the view when validation failed.
                return BadRequest(ModelState);
            }
            var newRequest = _mapper.Map<Request>(request);
            return Ok();
        }

        [HttpPut]
        public async Task<ActionResult> Put([FromBody] Models.Request.RequestRequest request)
        {
            if (!ModelState.IsValid)
            { // re-render the view when validation failed.
                return BadRequest(ModelState);
            }
            var newRequest = _mapper.Map<Request>(request);
            return Ok();
        }

        [HttpDelete]
        public async Task<ActionResult> Delete([FromBody] Models.Request.RequestRequest request)
        {
            if (!ModelState.IsValid)
            { // re-render the view when validation failed.
                return BadRequest(ModelState);
            }
            var newRequest = _mapper.Map<Request>(request);
            return Ok();
        }
    }
}