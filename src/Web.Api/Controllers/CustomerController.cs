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
using Web.Api.Core.Dto.UseCaseResponses;
using System.Web.Http.Cors;

namespace Web.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors(origins: "http://localhost:3000", headers: "Access-Control-Allow-Origin", methods: "*")]
    public class CustomerController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ICustomerRepository _repository;
        private readonly ICRUDCustomerUseCase _getCustomerUseCase;
        private readonly IExportCustomerUseCase _exportCustomerUseCase;
        private readonly CustomerPresenter _customerPresenter;
        private readonly ExportCustomerPresenter _exportPresenter;

        public CustomerController(IMapper mapper, ICustomerRepository repository, ICRUDCustomerUseCase getCustomerUseCase,
            IExportCustomerUseCase exportCustomerUseCase, CustomerPresenter customerPresenter, ExportCustomerPresenter exportPresenter)
        {
            _mapper = mapper;
            _repository = repository;
            _getCustomerUseCase = getCustomerUseCase;
            _exportCustomerUseCase = exportCustomerUseCase;
            _customerPresenter = customerPresenter;
            _exportPresenter = exportPresenter;
        }

        
        [HttpGet]
        
        public async Task<IEnumerable<Object>> GetCustomerList()
        {
            //var CustomerItems = _repository.GetCustomerList();
            //return Ok(_mapper.Map<IEnumerable<ModelRequest.CustomerRequest>>(CustomerItems));
            return await _repository.GetCustomerList();
        }

        // GET api/<UsersController>/5
        /*[HttpGet("{id}")]
        public async Task<Customer> Get(string id)
        {
            return await _repository.FindById(id);
        }*/

        [HttpGet("{keyword}")]
        public async Task<IEnumerable<Object>> Filter(string keyword)
        {
            return await _repository.Filter(keyword);
        }

        [HttpGet("server")]
        public async Task<IEnumerable<Object>> GetAllServer()
        {
            return await _repository.GetAllServer();
        }

        [HttpGet("server/{keyword}")]
        public async Task<IEnumerable<Object>> FilterServer(string keyword)
        {
            return await _repository.FilterServer(keyword);
        }

        [HttpGet("server/id={id}")]
        public async Task<IEnumerable<Object>> GetOwnedServer(string id)
        {
            return await _repository.GetOwnedServer(id);
        }

        [HttpGet("server/available")]
        public async Task<IEnumerable<Object>> GetAvailableServer()
        {
            return await _repository.GetAvailableServer();
        }

        [HttpGet("contact-point")]
        public async Task<IEnumerable<Object>> GetAllContactPoint()
        {
            return await _repository.GetAllContactPoint();
        }

        [HttpPost("export-csv")]
        public async Task<ActionResult> GetByCustomers(ExportCustomerRequest request)
        {
            if (!ModelState.IsValid)
            { // re-render the view when validation failed.
                return BadRequest(ModelState);
            }
            await _exportCustomerUseCase.Handle(new
                UseCaseRequest.ExportCustomerRequest(request.FromDate, request.ToDate, request.Guids), _exportPresenter);
            return _exportPresenter.ContentResult;
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] UseCaseRequest.CustomerRequest request)
        {
            if (!ModelState.IsValid)
            { // re-render the view when validation failed.
                return BadRequest(ModelState);
            }
            await _getCustomerUseCase.Handle(new UseCaseRequest.CustomerRequest(request.Name, request.ContractBeginDate, request.ContractBeginDate, request.ContactPoint, 
                request.Description, request.Status), _customerPresenter);
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