using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Web.Api.Core.Domain.Entities;
using Web.Api.Core.Dto;
using Web.Api.Core.Dto.GatewayResponses.Repositories;
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
            var startDate = new SqlParameter("@StartDate", server.StartDate);
            var endDate = new SqlParameter("@EndDate", server.EndDate);
            _context.Database.ExecuteSqlCommand(" EXEC dbo.CreateServer @Name, @IpAddress, @CreatedBy, @StartDate, @EndDate ", name, ipAdress, createdBy, startDate, endDate );
            var success = await _context.SaveChangesAsync();
            return new CRUDServerResponse(server.Id, success > 0, null);

        }

        //READ
        public IEnumerable<Core.Domain.Entities.Server> GetAllCommand()
        {
            return _context.Server.ToList();
        }

        //UPDATE
        public async Task<CRUDServerResponse> UpdateServer(Server server)
        {
            var name = new SqlParameter("@Name", server.Name);
            var ipAddress = new SqlParameter("@IpAddress", server.IpAddress);
            var id = new SqlParameter("@Id", server.Id);
            var idupdateBy = new SqlParameter("@IdUpdateBy", server.UpdatedBy);
            var startDate = new SqlParameter("@StartDate", server.StartDate);   
            var endDate = new SqlParameter("@EndDate", server.EndDate);
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
        public async Task<CRUDServerResponse> UpdateMutilServerStatus( DataTable serverIdList, bool status, Guid userId)
        {
            var _serverIdList = new SqlParameter("@ServerIds", serverIdList);
            _serverIdList.SqlDbType = SqlDbType.Structured;
            _serverIdList.TypeName = "dbo.IdList";
            var _status = new SqlParameter("@Status", status);
            var _userId = new SqlParameter("@Updator", userId);
            _context.Database.ExecuteSqlCommand("EXEC dbo.UpdateMutilServerStatus @ServerIds, @Status, @Updator", _serverIdList, _status, _userId);
            var success = await _context.SaveChangesAsync();
            return new CRUDServerResponse(userId, success > 0, null);
        }
    }
}
