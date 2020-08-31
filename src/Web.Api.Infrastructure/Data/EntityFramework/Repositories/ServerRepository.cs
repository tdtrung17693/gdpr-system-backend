using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Internal;
using Web.Api.Infrastructure.Data.Entities;
using Web.Api.Core.Domain.Entities;
using Web.Api.Core.Dto;
using Web.Api.Core.Dto.GatewayResponses.Repositories;
using Web.Api.Core.Dto.UseCaseResponses;
using Web.Api.Core.Interfaces.Gateways.Repositories;
//using Web.Api.Infrastructure.Data.Entities;

namespace Web.Api.Infrastructure.Data.EntityFramework.Repositories
{
    internal sealed class ServerRepository : IServerRepository
    {
        //private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;
        private readonly GdprContext _context;

        public ServerRepository(IMapper mapper, GdprContext context){ 
            _mapper = mapper;
            _context = context;
        }

        //CREATE 
        public async Task<CRUDServerResponse> Create(Core.Domain.Entities.Server server)
        {
            // var newServer = _mapper.Map<Data.Entities.Server>(server);
            //var identityResult = await _context.Server.CreateAsync(newServer);
            await _context.Server.AddAsync(server);
            var success = await _context.SaveChangesAsync();
            return new CRUDServerResponse(server.Id, success > 0, null);
        }

        //READ
        public IEnumerable<Core.Domain.Entities.Server> GetAllCommand()
        {
            //var serverList = await _context.Server; 
            //var listServer = _mapper.Map<IEnumerable<Data.Entities.Server>>(_context.Server.ToList());
            //return _mapper.Map<IEnumerable<Core.Domain.Entities.Server>>(listServer);
            return _context.Server.ToList();
        }
    }
}
