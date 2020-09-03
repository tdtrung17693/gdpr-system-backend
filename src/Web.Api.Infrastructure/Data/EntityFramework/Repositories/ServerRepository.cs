using System;
using System.Collections.Generic;
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
            _context.Database.ExecuteSqlCommand(" EXEC dbo.CreateServer @Name, @IpAddress ", name, ipAdress);
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
            var name = new SqlParameter("@Name", "Long net.1999");
            var ipAddress = new SqlParameter("@IpAddress", "27.07");
            var id = new SqlParameter("@Id", Guid.Parse("4F2EEC9C-A3A3-416C-851B-947CD48FEF3E"));
            var idupdateBy = new SqlParameter("@IdUpdateBy", Guid.Parse("875CE677-0DB1-4482-9B5C-5862E264C967"));
            var updateAt = new SqlParameter("@UpdateAt", Convert.ToDateTime("2019-12-30"));
            var startDate = new SqlParameter("@StartDate", Convert.ToDateTime("2019-12-30"));   
            var endDate = new SqlParameter("@EndDate", Convert.ToDateTime("2020-7-27"));
            var status = new SqlParameter("@Status", true);

            _context.Database.ExecuteSqlCommand("EXEC UpdateServer @Id, @IdUpdateBy, @UpdateAt, @Name, @IpAddress, @StartDate, @EndDate, @Status", id, idupdateBy, updateAt ,name, ipAddress, startDate, endDate, status);
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
    }
}
