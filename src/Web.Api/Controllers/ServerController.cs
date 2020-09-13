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
using OfficeOpenXml;
using Microsoft.AspNetCore.Hosting;

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
        private readonly IHostingEnvironment _hostingEnvironment;

        public ServerController(IHostingEnvironment hostingEnvironment, IMapper mapper, IServerRepository repository, ICreateServerUseCase createServerUseCase, 
            CreateServerPresenter createServerPresenter, UpdateServerPresenter updateServerPresenter ,IUpdateServerUseCase updateServerUseCase,
            IBulkServerUseCase bulkServerUseCase, BulkServerPresenter bulkServerPresenter)//, ICreateServerUseCase createServerUseCase, CreateServerPresenter createServerPresenter   
        {
            _hostingEnvironment = hostingEnvironment;
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
            await _createServerUseCase.Handle(new CreateServerRequest(server.id, server.CreatedAt, server.CreatedBy, server.DeletedAt, server.DeletedBy, server.EndDate,
            server.IpAddress, server.IsDeleted, server.Name,
             server.StartDate, server.Status, server.UpdatedAt, server.UpdatedBy), _createServerPresenter);
            return Ok("You hav add an row");
           
           
        }

        //READ
        [EnableCors("server")]
        [HttpGet]
        public ActionResult<IEnumerable<ServerRequest>> GetAllCommands()
        {
            var serverItems = _repository.GetAllCommand();
            return Ok(_mapper.Map<IEnumerable<ServerRequest>>(serverItems));
        }

        //UPDATE
        [EnableCors("server")]
        [HttpPut]
        public async Task<ActionResult> UpdateServer([FromBody] ServerUpdateRequest server)
        {

            if (!ModelState.IsValid) 
            { 
                return BadRequest(ModelState);
            }
            await _updateServerUseCase.Handle(new UpdateServerRequest(server.id, server.CreatedAt, server.CreatedBy, server.DeletedAt, server.DeletedBy, server.EndDate,
            server.IpAddress, server.IsDeleted, server.Name,
             server.StartDate, server.Status, server.UpdatedAt, server.UpdatedBy) , _updateServerPresenter);
            return Ok("You hav update an row");
        }

        //Get detail a server
        [EnableCors("server")]
        [HttpGet("detail/{id}")]
        public ActionResult<ServerRequest> GetServerDetail(Guid id)
        {
            var serverItem = _repository.GetServerDetail(id);
            return Ok(_mapper.Map<ServerRequest>(serverItem));
        }


        //Active/Deactive multi server
        [EnableCors("server")]
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

        //Import Server
        [EnableCors("server")]
        [HttpPost("{id}/import")]
        public async Task<ServerImportResponse<List<ServerImportRequest>>> ImportMultiStatusServer(Guid id,IFormFile formFile, CancellationToken cancellationToken)//IEnumerable<Guid> serverIdList,bool status, Guid updator
        {


            if (formFile == null || formFile.Length <= 0)
            {
                return ServerImportResponse<List<ServerImportRequest>>.GetResult(-1, "formfile is empty");
            }

            if (!Path.GetExtension(formFile.FileName).Equals(".xlsx", StringComparison.OrdinalIgnoreCase))
            {
                return ServerImportResponse<List<ServerImportRequest>>.GetResult(-1, "Not Support file extension");
            }

            var list = new List<ServerImportRequest>();

            using (var stream = new MemoryStream())
            {
                await formFile.CopyToAsync(stream, cancellationToken);

                using (var package = new ExcelPackage(stream))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                    var rowCount = worksheet.Dimension.Rows;

                    for (int row = 2; row <= rowCount; row++)
                    {
                        list.Add(new ServerImportRequest
                        {
                            Name = worksheet.Cells[row, 1].Value.ToString().Trim(),
                            IpAddress = worksheet.Cells[row, 2].Value.ToString().Trim(),
                            StartDate = DateTime.FromOADate(double.Parse(worksheet.Cells[row, 3].Value.ToString())),
                            EndDate = DateTime.FromOADate(double.Parse(worksheet.Cells[row, 4].Value.ToString())),
                            CreatedBy = id

                        }) ;

                        await _createServerUseCase.Handle(new CreateServerRequest(Guid.NewGuid(), null, list[list.Count - 1].CreatedBy, null, null, list[list.Count - 1].EndDate,
                         list[list.Count - 1].IpAddress, false, list[list.Count - 1].Name,
                            list[list.Count - 1].StartDate, true, null, null), _createServerPresenter);
                    }
                }
            }
            return ServerImportResponse<List<ServerImportRequest>>.GetResult(200, "OK", list);
        }

    }
}
