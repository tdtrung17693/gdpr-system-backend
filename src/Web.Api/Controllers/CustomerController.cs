using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Web.Api.Core.Domain.Entities;
using UseCaseRequest = Web.Api.Core.Dto.UseCaseRequests;
using Web.Api.Core.Interfaces.Gateways.Repositories;
using Web.Api.Core.Interfaces.UseCases.ICustomerUseCases;
using ModelRequest = Web.Api.Models.Request;
using Web.Api.Presenters;
using Web.Api.Core.Dto.UseCaseRequests;
using Web.Api.Core.Dto.UseCaseResponses;
using System.Web.Http.Cors;
using Web.Api.Core.UseCases.CustomerUseCases;
using Microsoft.AspNetCore.Authorization;
using Web.Api.Core.Dto.UseCaseRequests.CustomerUseCaseRequest;

namespace Web.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ICustomerRepository _repository;
        private readonly ICRUDCustomerUseCase _getCustomerUseCase;
        private readonly IExportCustomerUseCase _exportCustomerUseCase;
        private readonly IManageServerUseCase _manageServerUseCase;
        private readonly CustomerPresenter _customerPresenter;
        private readonly ExportCustomerPresenter _exportPresenter;
        private readonly ManageServerPresenter _serverPresenter;

        public CustomerController(IMapper mapper, ICustomerRepository repository, ICRUDCustomerUseCase getCustomerUseCase, IExportCustomerUseCase exportCustomerUseCase, 
            IManageServerUseCase manageServerUseCase, CustomerPresenter customerPresenter, ExportCustomerPresenter exportPresenter, ManageServerPresenter serverPresenter)
        {
            _mapper = mapper;
            _repository = repository;
            _getCustomerUseCase = getCustomerUseCase;
            _exportCustomerUseCase = exportCustomerUseCase;
            _manageServerUseCase = manageServerUseCase;
            _customerPresenter = customerPresenter;
            _exportPresenter = exportPresenter;
            _serverPresenter = serverPresenter;
        }

        
        [HttpGet]
        
        public async Task<IEnumerable<Object>> GetCustomerList()
        {
            //var CustomerItems = _repository.GetCustomerList();
            //return Ok(_mapper.Map<IEnumerable<ModelRequest.CustomerRequest>>(CustomerItems));
            return await _repository.GetCustomerList();
        }

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
                ExportCustomerRequest(request.FromDate, request.ToDate, request.Guids), _exportPresenter);
            return _exportPresenter.ContentResult;
        }

        [HttpPost]
        [Authorize("CanCreateCustomer")]
        public async Task<ActionResult> Post([FromBody] CustomerRequest request)
        {
            if (!ModelState.IsValid)
            { // re-render the view when validation failed.
                return BadRequest(ModelState);
            }
            await _getCustomerUseCase.Handle(new CustomerRequest(request.Name, request.ContractBeginDate, request.ContractEndDate, request.ContactPoint, 
                request.Description, request.Status), _customerPresenter);
            return _customerPresenter.ContentResult;
        }

        [HttpPost("server")]
        public async Task<ActionResult> Post(ManageServerRequest request)
        {
            if (!ModelState.IsValid)
            { // re-render the view when validation failed.
                return BadRequest(ModelState);
            }
            await _manageServerUseCase.Handle(new ManageServerRequest(request.CustomerId, request.ServerIds, request.Action), _serverPresenter);
            return _serverPresenter.ContentResult;
        }

        [HttpDelete("server")]
        public async Task<ActionResult> Delete([FromBody] ManageServerRequest request)
        {
            if (!ModelState.IsValid)
            { // re-render the view when validation failed.
                return BadRequest(ModelState);
            }
            await _manageServerUseCase.Handle(new ManageServerRequest(request.CustomerId, request.ServerIds, request.Action), _serverPresenter);
            return _serverPresenter.ContentResult;
        }

        [HttpPut]
        public async Task<ActionResult> Put([FromBody] CustomerRequest request)
        {
            if (!ModelState.IsValid)
            { // re-render the view when validation failed.
                return BadRequest(ModelState);
            }
            /*Customer newCustomer = new Customer(request.Name, req
             * uest.ContractBeginDate, request.ContractBeginDate, request.ContactPoint, request.Description);
            return Ok(await _repository.Update(newCustomer));*/
            await _getCustomerUseCase.Handle(new CustomerRequest(request.Name, request.ContractBeginDate, request.ContractEndDate, request.ContactPoint, request.Description, request.Status, 
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