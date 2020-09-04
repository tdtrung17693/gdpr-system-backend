using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Web.Api.Core.Domain.Entities;
using UseCaseRequest = Web.Api.Core.Dto.UseCaseRequests;
using Web.Api.Core.Interfaces.Gateways.Repositories;
using Web.Api.Core.Interfaces.UseCases;
using ModelRequest = Web.Api.Models.Request;
using Web.Api.Presenters;
using Web.Api.Core.Dto.UseCaseRequests;

namespace Web.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ICustomerRepository _repository;
        private readonly ICRUDCustomerUseCase _getCustomerUseCase;
        private readonly CustomerPresenter _customerPresenter;

        public CustomerController(IMapper mapper, ICustomerRepository repository, ICRUDCustomerUseCase getCustomerUseCase, CustomerPresenter customerPresenter)
        {
            _mapper = mapper;
            _repository = repository;
            _getCustomerUseCase = getCustomerUseCase;
            _customerPresenter = customerPresenter;
        }


        [HttpGet]
        public async Task<IEnumerable<Customer>> GetCustomerList()
        {
            //var CustomerItems = _repository.GetCustomerList();
            //return Ok(_mapper.Map<IEnumerable<ModelRequest.CustomerRequest>>(CustomerItems));
            return await _repository.GetCustomerList();
        }

        // GET api/<UsersController>/5
        [HttpGet("{id}")]
        public async Task<Customer> Get(string id)
        {
            return await _repository.FindById(id);
        }

        /*[HttpGet("List")]
        public async Task<Request> GetByCustomers(string id)
        {
            return await _repository.(id);
        }*/

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] Models.Request.CustomerRequest request)
        {
            if (!ModelState.IsValid)
            { // re-render the view when validation failed.
                return BadRequest(ModelState);
            }
            await _getCustomerUseCase.Handle(new UseCaseRequest.CustomerRequest(request.Name, request.ContractBeginDate, request.ContractBeginDate, request.ContactPoint, request.Description, request.Status), _customerPresenter);
            return _customerPresenter.ContentResult;
        }

        [HttpPut]
        public async Task<ActionResult> Put([FromBody] Models.Request.CustomerRequest request)
        {
            if (!ModelState.IsValid)
            { // re-render the view when validation failed.
                return BadRequest(ModelState);
            }
            /*Customer newCustomer = new Customer(request.Name, req
             * uest.ContractBeginDate, request.ContractBeginDate, request.ContactPoint, request.Description);
            return Ok(await _repository.Update(newCustomer));*/
            await _getCustomerUseCase.Handle(new UseCaseRequest.CustomerRequest(request.Name, request.ContractBeginDate, request.ContractBeginDate, request.ContactPoint, request.Description, request.Status, 
                request.Id), _customerPresenter);
            return _customerPresenter.ContentResult;
        }

        [HttpDelete]
        public async Task<ActionResult> Delete([FromBody] Models.Request.CustomerRequest request)
        {
            if (!ModelState.IsValid)
            { // re-render the view when validation failed.
                return BadRequest(ModelState);
            }
            Customer newCustomer = new Customer(request.Name, request.ContractBeginDate, request.ContractBeginDate, request.ContactPoint, request.Description, request.Status, Guid.NewGuid());
            return Ok(await _repository.Delete(newCustomer));
        }
    }
}