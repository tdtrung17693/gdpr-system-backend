using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Web.Api.Core.Domain.Entities;
using Web.Api.Core.Dto.UseCaseRequests;
using Web.Api.Core.Interfaces.Gateways.Repositories;
using Web.Api.Core.Interfaces.UseCases;
using Web.Api.Models.Request;
using Web.Api.Presenters;

namespace Web.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ICustomerRepository _repository;

        public CustomerController(IMapper mapper, ICustomerRepository repository)
        {
            _mapper = mapper;
            _repository = repository;
        }


        [HttpGet]
        public ActionResult<IEnumerable<CustomerRequest>> GetCustomerList()
        {
            var CustomerItems = _repository.GetCustomerList();
            return Ok(_mapper.Map<IEnumerable<CustomerRequest>>(CustomerItems));
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] Models.Request.CustomerRequest request)
        {
            if (!ModelState.IsValid)
            { // re-render the view when validation failed.
                return BadRequest(ModelState);
            }
            var newCustomer = _mapper.Map<Customer>(request);
            return Ok(await _repository.Create(newCustomer)); 
        }
    }
}