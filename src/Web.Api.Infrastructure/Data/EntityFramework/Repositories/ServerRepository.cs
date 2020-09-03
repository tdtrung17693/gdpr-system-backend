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
            
            var name = new SqlParameter("@Name", "Long netpower.1999");
            var ipAdress = new SqlParameter("@IpAddress", "1999.1999");
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
        public async Task<CRUDServerResponse> UpdateServer()
        {
            var name = new SqlParameter("@Name", "Long net.1999");
            var ipAddress = new SqlParameter("@IpAddress", "27.07");
            /*var id = new SqlParameter("@Id",Guid.Parse("D2636F26-E649-4A74-B1A5-F8BABD80036B"));
            var idupdateBy = new SqlParameter("@IdUpdateBy",Guid.Parse("2E918B64-9E0D-4865-9CBF-23F5BA26EB2E"));
            var updateAt = new SqlParameter("@UpdateAt", Convert.ToDateTime("2019-12-30"));
            var startDate = new SqlParameter("@StartDate", Convert.ToDateTime("2019-12-30"));
            var endDate = new SqlParameter("@EndDate", Convert.ToDateTime("2020-7-27"));
            var status = new SqlParameter("@Status", 1);
*/
            _context.Database.ExecuteSqlCommand("EXEC UpdateServer  @Name,@IpAddress", name, ipAddress);
            var success = await _context.SaveChangesAsync();
            return new CRUDServerResponse(null, success > 0, null);

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
