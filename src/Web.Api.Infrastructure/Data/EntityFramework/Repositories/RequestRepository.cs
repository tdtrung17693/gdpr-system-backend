﻿using System;
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
                toDate.Value = (object)request.EndDate ?? DBNull.Value;
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

                    await _eventBus.Trigger(requestCreatedEvent);
                    return new CreateRequestResponse(requestCreatedEvent.RequestId, true);
                }
                catch (Exception e)
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
                var server = new SqlParameter("@ServerId", request.ServerId);
                var description = new SqlParameter("@Description", request.Description);
                var updateBy = new SqlParameter("@UpdatedBy", request.UpdatedBy);
                var updateAt = new SqlParameter("@UpdatedAt", Convert.ToDateTime(DateTime.Now));
                
                var command = _context.Database.GetDbConnection().CreateCommand();
                command.CommandText = "UpdateRequest";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(id);
                command.Parameters.Add(title);
                command.Parameters.Add(fromDate);
                command.Parameters.Add(toDate);
                command.Parameters.Add(server);
                command.Parameters.Add(description);
                command.Parameters.Add(updateBy);
                command.Parameters.Add(updateAt);
                
                if (command.Connection.State == ConnectionState.Closed)
                    await command.Connection.OpenAsync();
                
                var resultReader = await command.ExecuteReaderAsync();

                var updatedFields = new Dictionary<string, List<string>>();
                while (resultReader.Read())
                {
                    updatedFields.Add(
                        Convert.ToString(resultReader["UpdatedField"]), 
                        new List<string>
                        {
                            Convert.ToString(resultReader["PreviousState"]), 
                            Convert.ToString(resultReader["UpdatedState"])
                        });
                }

                await _eventBus.Trigger(new RequestUpdated()
                {
                    UpdatedFields = updatedFields,
                    RequestId = (Guid) request.Id,
                    UpdatedBy = (Guid) request.UpdatedBy
                });
                
                var success = await _context.SaveChangesAsync();
                return new UpdateRequestResponse(request.Id.ToString(), success > 0, null);
            }

            public async Task<IList<RequestDetail>> GetRequest(Guid? Uid, int PageNo, int PageSize, string Keyword, string FilterStatus)
            {
                var parameters = new List<SqlParameter>();
                var uid = new SqlParameter("@uid",Uid);
                parameters.Add(uid);
                var pageNo = new SqlParameter("@PageNumber", PageNo);
                parameters.Add(pageNo);
                var pageSize = new SqlParameter("@RowsOfPage", PageSize);
                parameters.Add(pageSize);
                var sql = "";
                if (Keyword != null)
                {
                    var keyword = new SqlParameter("@Keyword", Keyword);
                    parameters.Add(keyword);
                    var filterStatus = new SqlParameter("@FilterStatus", FilterStatus);
                    parameters.Add(filterStatus);
                    sql = "EXEC GetRequestPagination @uId=@uid, @SearchKey=@Keyword, @PageNo=@PageNumber, @PageSize=@RowsOfPage, @FilterStatusString=@FilterStatus";
                }
                else
                {
                    var filterStatus = new SqlParameter("@FilterStatus", FilterStatus);
                    parameters.Add(filterStatus);
                    sql = "EXEC GetRequestPagination @uId=@uid, @PageNo=@PageNumber, @PageSize=@RowsOfPage, @FilterStatusString=@FilterStatus";
                }
                
                List<SPRequestResultView> resultRequestPaging = await _context.SPRequestResultView .FromSql(sql, parameters.ToArray()).ToListAsync();
                
                if (resultRequestPaging != null) return _mapper.Map<List<SPRequestResultView>, IList<RequestDetail>>(resultRequestPaging);
                return null;
            }

            public RequestDetail getEachRequest(string requestId, string role)
            {
                var parameters = new List<SqlParameter>();
                var rId = new SqlParameter("@rId", requestId);
                parameters.Add(rId);
                var sqlQuery = "EXEC GetEachRequest @IdRequest=@rId";
                List<SPRequestResultView> resultEachRequest = _context.SPRequestResultView.FromSql(sqlQuery, parameters.ToArray()).ToList();
                resultEachRequest[0].RoleName = role;
                if (resultEachRequest != null) return _mapper.Map<RequestDetail>(resultEachRequest[0]);
                return null;
            }

            public DataTable getNoRows(string searchKey)
            {
                using (var command = _context.Database.GetDbConnection().CreateCommand())
                {
                    searchKey = searchKey == null ? "" : searchKey;
                    var filterBy = new SqlParameter("@SearchKey", searchKey);
                    command.CommandText = "GetRequestCount";
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(filterBy);
                    DataTable dt = new DataTable();
                    _context.Database.OpenConnection();
                    dt.Load(command.ExecuteReader());
                    return dt;
                }
            }

            public async Task<IList<ExportRequestDetail>> GetRequestForExport(ExportRequest request)
            {
                var parameters = new List<SqlParameter>();
                var fromDate = new SqlParameter("@FromDateInput", request.fromDate);
                parameters.Add(fromDate);
                var toDate = new SqlParameter("@ToDateInput", request.toDate);
                parameters.Add(toDate);
                var  sql = "EXEC GetRequestExport @FromDate=@FromDateInput, @ToDate=@ToDateInput";
                List<SPRequestResultExportView> result = await _context.SPRequestResultExportView.FromSql(sql, parameters.ToArray()).ToListAsync();
                if (result != null) return _mapper.Map<List<SPRequestResultExportView>, IList<ExportRequestDetail>>(result);
                return null;
            }

            public async Task<IList<ExportRequestDetail>> exportBulkRequest(BulkExportRequest message)
            {
                var parameters = new List<SqlParameter>();
                var param3 = new SqlParameter("@RequestList", message.IdList);
                parameters.Add(param3);
                var sql = "EXEC GetExportBulkRequest @RequestList";
                List<SPRequestResultExportView> result = await _context.SPRequestResultExportView.FromSql(sql, parameters.ToArray()).ToListAsync();
                if (result != null) return _mapper.Map<List<SPRequestResultExportView>, IList<ExportRequestDetail>>(result);
                return null;
            }

            public async Task<bool> ManageRequest(ManageRequestRequest message)
            {
                var rId = new SqlParameter("@RequestId", message.RequestId);
                var uId = new SqlParameter("@UserId", message.UserId);
                var response = new SqlParameter("@Response", message.Answer);
                var status = new SqlParameter("@Status", message.Status);
                
                var command = _context.Database.GetDbConnection().CreateCommand();
                command.CommandText = "RequestManage";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(rId);
                command.Parameters.Add(uId);
                command.Parameters.Add(response);
                command.Parameters.Add(status);
                
                if (command.Connection.State == ConnectionState.Closed)
                    await command.Connection.OpenAsync();
                
                var resultReader = await command.ExecuteReaderAsync();
                await resultReader.ReadAsync();

                await _eventBus.Trigger(new RequestAcceptedRejected()
                {
                    OldStatus = (string)resultReader["OldStatus"],
                    NewStatus = message.Status,
                    RequestId = Guid.Parse(message.RequestId),
                    UpdatedBy = Guid.Parse(message.UserId),
                    ApproverFullName = (string)resultReader["ApproverFullName"],
                    RequesterFullName = (string)resultReader["RequesterFullName"],
                    RequesterId = (Guid)resultReader["RequesterId"],
                    RequesterUsername = (string)resultReader["RequesterUsername"],
                    RequesterEmail = (string)resultReader["RequesterEmail"],
                    RequestTitle = (string)resultReader["Title"]
                });

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
