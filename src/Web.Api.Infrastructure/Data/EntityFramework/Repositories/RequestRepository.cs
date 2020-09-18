using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using AutoMapper;
using Web.Api.Core.Domain.Entities;
using Web.Api.Core.Dto;
using Web.Api.Core.Dto.GatewayResponses.Repositories;
using Web.Api.Core.Interfaces.Gateways.Repositories;
using System.Data.SqlClient;
using System.Data;
using Microsoft.EntityFrameworkCore;
using Web.Api.Core.Dto.UseCaseRequests;
using Web.Api.Core.UseCases;
using Web.Api.Core.Dto.UseCaseResponses;
using CreateRequestResponse = Web.Api.Core.Dto.GatewayResponses.Repositories.CreateRequestResponse;
using UpdateRequestResponse = Web.Api.Core.Dto.GatewayResponses.Repositories.UpdateRequestResponse;
using Web.Api.Core.Interfaces.Services.Event;
using Web.Api.Core.Domain.Event;

namespace Web.Api.Infrastructure.Data.EntityFramework.Repositories
{

    namespace Web.Api.Infrastructure.Data.EntityFramework.Repositories
    {
        internal sealed class RequestRepository : IRequestRepository
        {
            public readonly IMapper _mapper;
            private readonly IDomainEventBus _eventBus;
            private ApplicationDbContext _context;
            
            public RequestRepository(IMapper mapper, ApplicationDbContext context, IDomainEventBus eventBus)
            {
                _mapper = mapper;
                _context = context;
                _eventBus = eventBus;
            }

            public async Task<CreateRequestResponse> CreateRequest(Request request)
            {
                var createdBy = new SqlParameter("@CreatedBy", request.CreatedBy);
                var title = new SqlParameter("@Title",request.Title);
                var fromDate = new SqlParameter("@FromDate", request.StartDate);
                var toDate = new SqlParameter("@ToDate", request.EndDate);
                var server = new SqlParameter("@Server", request.ServerId);
                var description = new SqlParameter("@Description", request.Description);
                
                var command = _context.Database.GetDbConnection().CreateCommand();
                command.CommandText = "CreateRequest";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(createdBy);
                command.Parameters.Add(title);
                command.Parameters.Add(fromDate);
                command.Parameters.Add(toDate);
                command.Parameters.Add(server);
                command.Parameters.Add(description);
                
                var success = await _context.SaveChangesAsync();

                if (success == 0)
                {
                }
                if (command.Connection.State == ConnectionState.Closed)
                    await command.Connection.OpenAsync();
                try
                {
                    var requestReader = await command.ExecuteReaderAsync();
                    await requestReader.ReadAsync();
                    var requestCreatedEvent = new RequestCreated()
                    {
                        RequesterId = (Guid)requestReader["RequesterId"],
                        CreatedAt = Convert.ToDateTime(requestReader["CreatedAt"]),
                        RequesterFullName = Convert.ToString(requestReader["RequesterFullName"]),
                        RequesterUsername = Convert.ToString(requestReader["RequesterUsername"]),
                        RequestId = (Guid)requestReader["RequestId"],
                        ServerId = (Guid)requestReader["ServerId"],
                        ServerName = (string)requestReader["ServerName"],
                    };
                    _eventBus.Trigger(requestCreatedEvent);
                    _eventBus.Trigger(new RequestNotiToAdmin(
                        Convert.ToString(requestReader["RequesterFullName"]),
                        (string)requestReader["ServerName"],
                        (Guid)requestReader["RequestId"],
                        (Guid)requestReader["ServerId"],
                        Convert.ToDateTime(requestReader["CreatedAt"]))
                      );
                    _eventBus.Trigger(new CreateLog((Guid)requestReader["RequestId"], "", "Created", "", "", (Guid)requestReader["RequesterId"])); 
                    command.Connection.Close();
                   return new CreateRequestResponse(requestCreatedEvent.RequestId, true);
                }
                catch (SqlException e)
                {
                    // Unique constraint violation code number
                    Console.Error.WriteLine(e);
                    return new CreateRequestResponse(Guid.Empty, false, new[]
                        {new Error(Error.Codes.UNKNOWN, Error.Messages.UNKNOWN)});
                }
            }

            public async Task<UpdateRequestResponse> UpdateRequest(Request request)
            {
                var id = new SqlParameter("@Id", request.Id);
                var title = new SqlParameter("@Title", request.Title);
                var fromDate = new SqlParameter("@StartDate", request.StartDate);
                var toDate = new SqlParameter("@EndDate", request.EndDate);
                var server = new SqlParameter("@Server", request.ServerId);
                var description = new SqlParameter("@Description", request.Description);
                var requestStatus = new SqlParameter("@RequestStatus", request.RequestStatus);
                var response = new SqlParameter("@Response", request.Response);
                var approvedBy = new SqlParameter("@ApprovedBy", request.ApprovedBy);
                var updateBy = new SqlParameter("@UpdateBy", request.UpdatedBy);
                var updateAt = new SqlParameter("@UpdateAt", Convert.ToDateTime(DateTime.Now));
                _context.Database.ExecuteSqlCommand(" EXEC dbo.UpdateRequest @Id, @Title, @StartDate, @EndDate, @Server, @Description, @RequestStatus, @Response, @ApprovedBy, @UpdateBy, @UpdateAt ", id, title, fromDate, toDate, server, description, requestStatus, response, approvedBy, updateBy, updateAt);
                var success = await _context.SaveChangesAsync();
                
                var command = _context.Database.GetDbConnection().CreateCommand();

                if (success == 0)
                {
                }
                if (command.Connection.State == ConnectionState.Closed) await command.Connection.OpenAsync();

                _eventBus.Trigger(new CreateLog(request.Id, "", request.RequestStatus,"", "", request.UpdatedBy));

                command.Connection.Close();

                return new UpdateRequestResponse(request.Id.ToString(), success > 0, null);
            }

            public async Task<IList<RequestDetail>> GetRequest(int PageNo, int PageSize, string Keyword, string FilterStatus/*,
                                                                DateTime? FromDateExport = null, DateTime? ToDateExport = null*/)
            {
                var parameters = new List<SqlParameter>();
                var pageNo = new SqlParameter("@PageNumber", PageNo);
                parameters.Add(pageNo);
                var pageSize = new SqlParameter("@RowsOfPage", PageSize);
                parameters.Add(pageSize);
                var keyword = new SqlParameter("@Keyword", Keyword);
                parameters.Add(keyword);
                var filterStatus = new SqlParameter("@FilterStatus", FilterStatus);
                parameters.Add(filterStatus);
                var sql = "EXEC GetRequestPagination @SearchKey=@Keyword, @PageNo=@PageNumber, @PageSize=@RowsOfPage, @FilterStatusString=@FilterStatus";
                /*if (FromDateExport is null || ToDateExport is null)
                {
                    var fromDateExport = new SqlParameter("@FromDateExport", FromDateExport);
                    parameters.Add(fromDateExport);
                    var toDateExport = new SqlParameter("@ToDateExport", ToDateExport);
                    parameters.Add(toDateExport);
                    sql = "EXEC GetRequestPagination @SearchKey=@Keyword, @PageNo=@PageNumber, @PageSize=@RowsOfPage, @FilterStatusString=@FilterStatus, @FromDate=@FromDateExport, @ToDate=@ToDateExport";
                } */
                List<SPRequestResultView> resultRequestPaging = await _context.SPRequestResultView .FromSql(sql, parameters.ToArray()).ToListAsync();
                //Console.WriteLine(resultRequestPaging.ToString());
                if (resultRequestPaging != null) return _mapper.Map<List<SPRequestResultView>, IList<RequestDetail>>(resultRequestPaging);
                return null;
            }

            //public IList<Request> GetRequestFilter(string _keyword, int _pageNo, int _pageSize)
            //{
            //    var keyword = new SqlParameter("@Keyword", _keyword);
            //    var pageNo = new SqlParameter("@PageNumber", _pageNo);
            //    var pageSize = new SqlParameter("@RowsOfPage", _pageSize);
            //    var result = _context.Request.FromSql("EXEC GetRequestPaginationFilter @Keyword, @PageNumber, @RowsOfPage", keyword, pageNo, pageSize).ToList();
            //    return result;
            //}

            public RequestDetail getEachRequest(string requestId)
            {
                var parameters = new List<SqlParameter>();
                var rId = new SqlParameter("@rId", requestId);
                parameters.Add(rId);
                var sqlQuery = "EXEC GetEachRequest @IdRequest=@rId";
                List<SPRequestResultView> resultEachRequest = _context.SPRequestResultView.FromSql(sqlQuery, parameters.ToArray()).ToList();
                if (resultEachRequest != null) return _mapper.Map<RequestDetail>(resultEachRequest[0]);
                return null;
            }

            public async Task<int> getNoPages(int PageSize)
            {
                var parameters = new List<SqlParameter>();
                var pageSize = new SqlParameter("@PageSize", PageSize);
                parameters.Add(pageSize);
                var noPages = new SqlParameter("@NoPage", SqlDbType.Int);
                noPages.Direction = ParameterDirection.Output;
                parameters.Add(noPages);
                var sql = "EXEC RequestGetNoPages @PageSize, @NoPage OUT";
                await _context.Database.ExecuteSqlCommandAsync(sql, parameters.ToArray());
                return (int)noPages.Value;
            }

            public async Task<IList<ExportRequestDetail>> GetRequestForExport(ExportRequest request)
            {
                var parameters = new List<SqlParameter>();
                var fromDate = new SqlParameter("@FromDateInput", request.fromDate);
                parameters.Add(fromDate);
                var toDate = new SqlParameter("@ToDateInput", request.toDate);
                parameters.Add(toDate);
                var sql = "EXEC GetRequestExport @FromDate=@FromDateInput, @ToDate=@ToDateInput";
                List<SPRequestResultView> result = await _context.SPRequestResultView.FromSql(sql, parameters.ToArray()).ToListAsync();
                if (result != null) return _mapper.Map<List<SPRequestResultView>, IList<ExportRequestDetail>>(result);
                return null;
            }

            public async Task<bool> ManageRequest(ManageRequestRequest message)
            {
                var parameters = new List<SqlParameter>();

                var rId = new SqlParameter("@rId", message.RequestId);
                parameters.Add(rId);
                string mockUserID = "B2895635-180A-4AB3-B8A8-430BEA69301F";
                var uId = new SqlParameter("@uId", /*message.UserId*/ mockUserID);
                parameters.Add(uId);
                var response = new SqlParameter("@response", message.Answer);
                parameters.Add(response);
                var status = new SqlParameter("@status", message.Status);
                parameters.Add(status);

                var manageQuery = "EXEC RequestManage @rID, @uId, @response, @status";
                await _context.Database.ExecuteSqlCommandAsync(manageQuery, parameters.ToArray());

                return true;
            }

            public RequestRepository(IMapper mapper, ApplicationDbContext context)
            {
                _mapper = mapper;
                _context = context;
            }


            /*-----------------------------------------------------------------------------------------------------------------------------------*/
            public IEnumerable<Request> GetRequestList()
            {
                return _context.Request.ToList();
            }


            public async Task<CRUDRequestResponse> Create(Request request)
            {
                var creator = new Guid("9D2B0228-4D0D-4C23-8B49-01A698857709");
                var createdBy = new SqlParameter("@CreatedBy", creator);
                var title = new SqlParameter("@Title", request.Title);
                var fromDate = new SqlParameter("@FromDate", request.StartDate);
                var toDate = new SqlParameter("@ToDate", request.EndDate);
                var server = new SqlParameter("@Server", request.ServerId);
                var description = new SqlParameter("@Description", request.Description);
                await _context.Database.ExecuteSqlCommandAsync(" EXEC dbo.CreateRequest @CreatedBy, @Title, @Fromdate, @ToDate, @Server, @Description ", createdBy, title, fromDate, toDate, server, description);
                var success = await _context.SaveChangesAsync();
                
                return new CRUDRequestResponse(request.Id, success > 0, null);
            }


            public async Task<CRUDRequestResponse> Update(Request request)
            {
                var id = new SqlParameter("@Id", request.Id);
                var title = new SqlParameter("@Title", request.Title);
                var fromDate = new SqlParameter("@StartDate", request.StartDate);
                var toDate = new SqlParameter("@EndDate", request.EndDate);
                var server = new SqlParameter("@Server", request.ServerId);
                var description = new SqlParameter("@Description", request.Description);
                var requestStatus = new SqlParameter("@RequestStatus", request.RequestStatus);
                var response = new SqlParameter("@Response", request.Response);
                var approvedBy = new SqlParameter("@ApprovedBy", request.ApprovedBy);
                var updateBy = new SqlParameter("@UpdateBy", request.UpdatedBy);
                var updateAt = new SqlParameter("@UpdateAt", Convert.ToDateTime(DateTime.Now));
                _context.Database.ExecuteSqlCommand(" EXEC dbo.UpdateRequest @Id, @Title, @StartDate, @EndDate, @Server, @Description, @RequestStatus, @Response, @ApprovedBy, @UpdateBy, @UpdateAt ", id, title, fromDate, toDate, server, description, requestStatus, response, approvedBy, updateBy, updateAt);
                var success = await _context.SaveChangesAsync();
                return new CRUDRequestResponse(request.Id, success > 0, null);
            }

            public async Task<CRUDRequestResponse> Delete(Request request)
            {
                var id = new SqlParameter("@Id", request.Id);
                _context.Database.ExecuteSqlCommand(" EXEC dbo.DeleteRequest @Id ", id);
                var success = await _context.SaveChangesAsync();
                return new CRUDRequestResponse(request.Id, success > 0, null);
            }

        }
    }
}