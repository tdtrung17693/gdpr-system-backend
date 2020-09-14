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
                var tempCreatedBy = new SqlParameter("@CreatedBy", request.CreatedBy);
                var title = new SqlParameter("@Title",request.Title);
                var fromDate = new SqlParameter("@FromDate", request.StartDate);
                var toDate = new SqlParameter("@ToDate", request.EndDate);
                var server = new SqlParameter("@Server", request.ServerId);
                var description = new SqlParameter("@Description", request.Description);
                _context.Database.ExecuteSqlCommand(" EXEC dbo.CreateRequest @CreatedBy, @Title, @Fromdate, @ToDate, @Server, @Description ", tempCreatedBy, title, fromDate, toDate, server, description);
                var success = await _context.SaveChangesAsync();
                return new CreateRequestResponse(request.Id, success > 0, null);
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
                return new UpdateRequestResponse(request.Id.ToString(), success > 0, null);
            }

            public async Task<IList<RequestDetail>> GetRequest(int PageNo, int PageSize)
            {
                var parameters = new List<SqlParameter>();
                var pageNo = new SqlParameter("@PageNumber", PageNo);
                parameters.Add(pageNo);
                var pageSize = new SqlParameter("@RowsOfPage", PageSize);
                parameters.Add(pageSize);
                var sql = "EXEC GetRequestPagination @PageNo=@PageNumber, @PageSize=@RowsOfPage";
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
                
                if (success == 0)
                {
                    await _eventBus.Trigger(new RequestCreated(Guid.Empty, DateTime.UtcNow, (Guid)request.ServerId));
                }
                
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