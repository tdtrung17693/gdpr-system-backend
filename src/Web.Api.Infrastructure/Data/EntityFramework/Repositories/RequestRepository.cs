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

namespace Web.Api.Infrastructure.Data.EntityFramework.Repositories
{

    namespace Web.Api.Infrastructure.Data.EntityFramework.Repositories
    {
        internal sealed class RequestRepository : IRequestRepository
        {
            public readonly IMapper _mapper;
            private ApplicationDbContext _context;

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
                var title = new SqlParameter("@Title", request.Title);
                var fromDate = new SqlParameter("@FromDate", request.StartDate);
                var toDate = new SqlParameter("@ToDate", request.EndDate);
                var server = new SqlParameter("@Server", request.ServerId);
                var description = new SqlParameter("@Description", request.Description);
                _context.Database.ExecuteSqlCommand(" EXEC dbo.CreateRequest @Title, @Fromdate, @ToDate, @Server, @Description ", title, fromDate, toDate, server, description);
                var success = await _context.SaveChangesAsync();
                return new CRUDRequestResponse(request.Id, success > 0, null);
            }


            public async Task<CRUDRequestResponse> Update(Request request)
            {
                var id = new SqlParameter("@Id", request.Id);
                var title = new SqlParameter("@Title", request.Title);
                var fromDate = new SqlParameter("@FromDate", request.StartDate);
                var toDate = new SqlParameter("@ToDate", request.EndDate);
                var server = new SqlParameter("@Server", request.ServerId);
                var description = new SqlParameter("@Description", request.Description);
                var updateBy = new SqlParameter("@IdUpdateBy", request.UpdatedBy);
                var updateAt = new SqlParameter("@UpdateAt", Convert.ToDateTime(DateTime.Now));
                _context.Database.ExecuteSqlCommand(" EXEC dbo.UpdateRequest @Id,@Title, @Fromdate, @ToDate, @Server, @Description, @UpdateBy, @UpdateAt ", id, title, fromDate, toDate, server, description, updateBy, updateAt);
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

            // ACCEPT/DECLINE BULK OF REQUESTS
            public async Task<CRUDRequestResponse> UpdateBulkRequestStatus(DataTable requestIdList, bool status, Guid userId)
            {
                var _requestIdList = new SqlParameter("@RequestIds", requestIdList);
                _requestIdList.SqlDbType = SqlDbType.Structured;
                _requestIdList.TypeName = "dbo.IdList";
                var _status = new SqlParameter("@Status", status);
                var _userId = new SqlParameter("@Updator", userId);
                _context.Database.ExecuteSqlCommand("EXEC dbo.SetRequestsStatus @RequestIds, @Status, @Updator", _requestIdList, _status, _userId);
                var success = await _context.SaveChangesAsync();
                return new CRUDRequestResponse(userId, success > 0, null);
            }
        }
    }
}