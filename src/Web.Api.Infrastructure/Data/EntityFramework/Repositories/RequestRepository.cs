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
using Web.Api.Core.Interfaces.Services.Event;
using Web.Api.Core.Domain.Event;

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

            public IEnumerable<Request> GetRequestList()
            {
                return _context.Request.ToList();
            }


            public async Task<CRUDRequestResponse> Create(Request request)
            {
                var createdBy = new SqlParameter("@CreatedBy", request.CreatedBy);
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