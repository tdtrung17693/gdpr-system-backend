using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
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
        private readonly GdprContext _context;

        public ServerRepository(IMapper mapper, GdprContext context){ 
            _mapper = mapper;
            _context = context;
        }

        public IEnumerable<Server> GetAllCommand()
        {
            //var serverList = await _context.Server; 
            //var listServer = _mapper.Map<IEnumerable<Data.Entities.Server>>(_context.Server.ToList());
            //return _mapper.Map<IEnumerable<Core.Domain.Entities.Server>>(listServer);
            return _context.Server.ToList();
        }
    }
}
