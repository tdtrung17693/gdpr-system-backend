using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Web.Api.Core.Dto.UseCaseRequests;
using Web.Api.Core.Dto.UseCaseRequests.ServerUserCaseRequest;
using Web.Api.Core.Interfaces.Gateways.Repositories;
using Web.Api.Core.Interfaces.UseCases.ServerInterface;
using Web.Api.Core.UseCases;
using Web.Api.Models.Request;
using Web.Api.Presenters;
using BulkServerRequest = Web.Api.Core.Dto.UseCaseRequests.ServerUserCaseRequest.BulkServerRequest;
using Web.Api.Core.Dto.UseCaseResponses.ServerUseCaseResponse;
using System.IO;
using Microsoft.AspNetCore.Authorization;
using OfficeOpenXml;
using Microsoft.AspNetCore.Hosting;
using Web.Api.Core.Dto.UseCaseRequests.CustomerUseCaseRequest;
using Web.Api.Core.Interfaces.UseCases;
using Web.Api.Models.Request.Server;

namespace Web.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServerController : ControllerBase
    {   
        private readonly IMapper _mapper;
        private readonly IServerRepository _repository;
        private readonly ICreateServerUseCase _createServerUseCase;
        private readonly IReadServerUseCase _readServerUseCase;
        private readonly IUpdateServerUseCase _updateServerUseCase;
        private readonly IExportServerUseCase _exportServerUseCase;
        private readonly IBulkServerUseCase _bulkServerUseCase;
        private readonly CreateServerPresenter _createServerPresenter;
        private readonly UpdateServerPresenter _updateServerPresenter;
        private readonly BulkServerPresenter _bulkServerPresenter;
        private readonly ExportServerPresenter _exportServerPresenter;
        private readonly IHostingEnvironment _hostingEnvironment;
        

        public ServerController(IMapper mapper, IServerRepository repository, ICreateServerUseCase createServerUseCase, 
            CreateServerPresenter createServerPresenter, UpdateServerPresenter updateServerPresenter ,IUpdateServerUseCase updateServerUseCase,
            IBulkServerUseCase bulkServerUseCase, BulkServerPresenter bulkServerPresenter
            , IExportServerUseCase exportServerUseCase, ExportServerPresenter exportServerPresenter, IReadServerUseCase readServerUseCase)//, ICreateServerUseCase createServerUseCase, CreateServerPresenter createServerPresenter   
        {
            _mapper = mapper;
            _repository = repository;
            _createServerUseCase = createServerUseCase;
            _readServerUseCase = readServerUseCase;
            _updateServerUseCase = updateServerUseCase;
            _bulkServerUseCase = bulkServerUseCase;
            _createServerPresenter = createServerPresenter;
            _updateServerPresenter = updateServerPresenter;
            _bulkServerPresenter = bulkServerPresenter;
            _exportServerUseCase = exportServerUseCase;
            _exportServerPresenter = exportServerPresenter;
        }

        //CREATE
        [HttpPost("create")]
        [Authorize("CanCreateServer")]
        public async Task<ActionResult> CreateNewServer([FromBody] ServerRequest server)   
        {

            if (!ModelState.IsValid)
            { 
                return BadRequest(ModelState);
            }
            await _createServerUseCase.Handle(new CreateServerRequest(server.id, server.CreatedAt, server.CreatedBy, server.DeletedAt, server.DeletedBy, server.EndDate,
                                                server.IpAddress, server.IsDeleted, server.Name,
                                                server.StartDate, server.Status, server.UpdatedAt, server.UpdatedBy), _createServerPresenter);
            return Ok("You have added an row");
           
           
        }

        //CREATE NEW LIST SERVER
        [HttpPost("importExcel")]
        [Authorize("CanImportData")]
        public async Task<ActionResult> ImportServerByXLSX([FromBody] IEnumerable<ServerRequest> serverList)//IEnumerable<Guid> serverIdList,bool status, Guid updator
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            foreach (ServerRequest server in serverList)
            {
                await _createServerUseCase.Handle(new CreateServerRequest(server.id, server.CreatedAt, server.CreatedBy, server.DeletedAt, server.DeletedBy, server.EndDate,
                                                    server.IpAddress, server.IsDeleted, server.Name,
                                                    server.StartDate, server.Status, server.UpdatedAt, server.UpdatedBy), _createServerPresenter);
            }

            return Ok(serverList);
        }

        [HttpGet("listServer")]
        [Authorize("CanListServer")]
        public ActionResult<DataTable> GetListServer()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var dt = _repository.GetListServer();
            return dt;
        }

        [HttpGet("filter/{filterKey}")]
        [Authorize("CanViewServer")]
        public ActionResult<DataTable> GetListServerByFilter(string filterKey)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var dt = _repository.GetListServerByFilter(filterKey);
            return dt;
        }

        [HttpPost("count")]
        [Authorize("CanViewServer")]
        public ActionResult<DataTable> CountServers([FromBody]CountServerRequest filter)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var dt = _repository.CountServers(filter.filterString);
            return dt;
        }

        [HttpPost("paging")]
        [Authorize("CanViewServer")]
        public ActionResult<DataTable> Paging([FromBody] PagedServerRequest paged)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var dt = _repository.Paging(paged.page, paged.pageSize, paged.sortedBy, paged.sortOrder, paged.filterBy);
            return dt;
        }

        //UPDATE
        [HttpPut]
        [Authorize("CanEditServer")]
        public async Task<ActionResult> UpdateServer([FromBody] ServerUpdateRequest server)
        {

            if (!ModelState.IsValid) 
            { 
                return BadRequest(ModelState);
            }
            await _updateServerUseCase.Handle(new UpdateServerRequest(server.id, server.CreatedAt, server.CreatedBy, server.DeletedAt, server.DeletedBy, server.EndDate,
            server.IpAddress, server.IsDeleted, server.Name,
             server.StartDate, server.Status, server.UpdatedAt, server.UpdatedBy) , _updateServerPresenter);
            return Ok("Server has been successfully updated");
        }

        //Get detail a server
        [HttpGet("detail/{id}")]
        [Authorize("CanEditServer")]
        public ActionResult<ServerRequest> GetServerDetail(Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var serverItem = _repository.GetServerDetail(id);
            return Ok(_mapper.Map<ServerRequest>(serverItem));
        }


        //Active/Deactive multi server
        [HttpPut("bulkStatus")]
        [Authorize("CanEditServer")]
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
            var response = await _bulkServerUseCase.Handle(new BulkServerRequest(idList, bulkServer.updator) , _bulkServerPresenter);
            return Ok("Done");

        }

        [HttpPost("export-csv")]
        [Authorize("CanExportData")]
        public async Task<ActionResult> GetByCustomers(ExportCustomerRequest request)
        {
            if (!ModelState.IsValid)
            { // re-render the view when validation failed.
                return BadRequest(ModelState);
            }
            await _exportServerUseCase.Handle(new ExportServerRequest(request.FromDate, request.ToDate, request.Guids), _exportServerPresenter);
            return _exportServerPresenter.ContentResult;
        }
    }
}
