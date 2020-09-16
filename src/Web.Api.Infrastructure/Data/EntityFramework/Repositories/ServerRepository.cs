using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Web.Api.Core;
using Web.Api.Core.Domain.Entities;
using Web.Api.Core.Dto;
using Web.Api.Core.Dto.GatewayResponses.Repositories;
using Web.Api.Core.Dto.UseCaseRequests;
using Web.Api.Core.Interfaces.Gateways.Repositories;
//using Web.Api.Infrastructure.Data.Entities;

namespace Web.Api.Infrastructure.Data.EntityFramework.Repositories
{
    internal sealed class ServerRepository : IServerRepository
    {
        //private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;
        private readonly ApplicationDbContext _context;

        public ServerRepository(IMapper mapper, ApplicationDbContext context){ 
            _mapper = mapper;
            _context = context;
        }

        //CREATE 
        public async Task<CRUDServerResponse> Create(Server server)
        {
            
            var name = new SqlParameter("@Name", server.Name);
            var ipAdress = new SqlParameter("@IpAddress", server.IpAddress);
            var createdBy = new SqlParameter("@CreatedBy", server.CreatedBy);
            var startDate = new SqlParameter("@StartDate", null);
            startDate.Value = (object)server.StartDate ?? DBNull.Value;
            var endDate = new SqlParameter("@EndDate", null);
            endDate.Value = (object)server.EndDate ?? DBNull.Value;
            _context.Database.ExecuteSqlCommand(" EXEC dbo.CreateServer @Name, @IpAddress, @CreatedBy, @StartDate, @EndDate ", name, ipAdress, createdBy, startDate, endDate );
            var success = await _context.SaveChangesAsync();
            return new CRUDServerResponse(server.Id, success > 0, null);

        }

        //READ
        public IEnumerable<Core.Domain.Entities.Server> GetAllCommand()
        {
            return _context.Server.ToList();
        }

        //READ ALL
        public DataTable GetListServer()
        {
            using (var command = _context.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = "getListServer";
                command.CommandType = CommandType.StoredProcedure;
                DataTable dt = new DataTable();
                _context.Database.OpenConnection();
                dt.Load(command.ExecuteReader());
                return dt;  
            }       
        }

        //READ BY FILTER
        public DataTable GetListServerByFilter(string filter) {
            using (var command = _context.Database.GetDbConnection().CreateCommand())
            {
                var _filter = new SqlParameter("@filterKey", filter);
                // var commandText = "exec getListServerFilter @filterKey";
                command.CommandText = "getListServerFilter";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(_filter);
                DataTable dt = new DataTable();
                _context.Database.OpenConnection();
                dt.Load(command.ExecuteReader());
                return dt;
            }
        }

        //UPDATE
        public async Task<CRUDServerResponse> UpdateServer(Server server)
        {
            var name = new SqlParameter("@Name", server.Name);
            var ipAddress = new SqlParameter("@IpAddress", server.IpAddress);
            var id = new SqlParameter("@Id", server.Id);
            var idupdateBy = new SqlParameter("@IdUpdateBy", server.UpdatedBy);
            var startDate = new SqlParameter("@StartDate",null);
            startDate.Value = (object)server.StartDate ?? DBNull.Value; 
            var endDate = new SqlParameter("@EndDate",null);
            endDate.Value = (object)server.EndDate ?? DBNull.Value;
            var status = new SqlParameter("@Status", server.Status);

            _context.Database.ExecuteSqlCommand("EXEC UpdateServer @Id, @IdUpdateBy, @Name, @IpAddress, @StartDate, @EndDate, @Status", id, idupdateBy, name, ipAddress, startDate, endDate, status);
            var success = await _context.SaveChangesAsync();
            return new CRUDServerResponse(server.Id, success > 0, null);

        }

        //DELETE
        public async Task<CRUDServerResponse> DeleteServer(Server server)
        {

            var name = new SqlParameter("@Name", "Long net.1999");
            var ipAdress = new SqlParameter("@IpAddress", "1.1999.1999.1");
            _context.Database.ExecuteSqlCommand(" EXEC dbo.DeleteServer @Name, @IpAddress ", name, ipAdress);
            var success = await _context.SaveChangesAsync();
            return new CRUDServerResponse(server.Id, success > 0, null);

        }

        // GET DETAIL A SERVER
        public Server GetServerDetail(Guid serverId)
        {
            var _serverId = new SqlParameter("@Id", serverId);
            var server = _context.Server.FirstOrDefault(s => s.Id == serverId);
            //var success = await _context.SaveChangesAsync();
            return server;
        }

        // ACTIVE OR DEACTIVE MULTISERVER
        public async Task<CRUDServerResponse> UpdateMutilServerStatus( DataTable serverIdList, Guid userId)
        {
            var _serverIdList = new SqlParameter("@ServerIds", serverIdList);
            _serverIdList.SqlDbType = SqlDbType.Structured;
            _serverIdList.TypeName = "dbo.IdList";
            var _userId = new SqlParameter("@Updator", userId);
            _context.Database.ExecuteSqlCommand("EXEC dbo.UpdateMutilServerStatus @ServerIds, @Updator", _serverIdList, _userId);
            var success = await _context.SaveChangesAsync();
            return new CRUDServerResponse(userId, success > 0, null);
        }

        //export
        public async Task<ExportCSVByCustomerResponse> GetExportServers(ExportServerRequest request)
        {

            var response = (from u in _context.User
                            join s in _context.Server
                            on u.Id equals s.CreatedBy
                            where s.StartDate >= request.FromDate && s.EndDate <= request.ToDate 
                            select new
                            {
                                FirstName = u.FirstName,
                                LastName = u.LastName,
                                Name = s.Name,
                                IpAddress = s.IpAddress,
                                StartDate = s.StartDate,
                                EndDate = s.EndDate,
                            }
                            ).GroupBy(u => u.FirstName).ToList();
            return new ExportCSVByCustomerResponse(response, true, null);
        }

        public async Task<Server> FindById(Guid id)
        {
            Server server = await _context.Server
                                            .Where(s => s.Id == id)
                                            .FirstOrDefaultAsync();
            return server;
        }

        public IPagedCollection<Server> FindAll()
        {
            // TODO: Remove magic number
            return new PagedCollection<Server>(
                _context.Server.AsQueryable()
            );
        }
        public DataTable CountServers()
        {
            using (var command = _context.Database.GetDbConnection().CreateCommand())
            {
                // var commandText = "exec getListServerFilter @filterKey";
                command.CommandText = "GetServerCount";
                command.CommandType = CommandType.StoredProcedure;
                DataTable dt = new DataTable();
                _context.Database.OpenConnection();
                dt.Load(command.ExecuteReader());
                return dt;
            }
        }

        public DataTable Paging(int Page, int PageSize, string SortBy, bool SortOrder, string FilterBy)
        {
            using (var command = _context.Database.GetDbConnection().CreateCommand())
            {
                var page = new SqlParameter("@Page", Page);
                var pageSize = new SqlParameter("@PageSize", PageSize);
                var sortBy = new SqlParameter("@SortBy", SortBy);
                var sortOrder = new SqlParameter("@SortOrder", SortOrder);
                var filterBy = new SqlParameter("@FilterBy", FilterBy);
                // var commandText = "exec getListServerFilter @filterKey";
                command.CommandText = "GetServerPaging";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(page);
                command.Parameters.Add(pageSize);
                command.Parameters.Add(filterBy);
                command.Parameters.Add(sortBy);
                command.Parameters.Add(sortOrder);
                DataTable dt = new DataTable();
                _context.Database.OpenConnection();
                dt.Load(command.ExecuteReader());
                return dt;
            }
        }

    }
}
