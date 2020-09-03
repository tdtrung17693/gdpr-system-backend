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

namespace Web.Api.Infrastructure.Data.EntityFramework.Repositories
{

    namespace Web.Api.Infrastructure.Data.EntityFramework.Repositories
    {
        internal sealed class RequestRepository : IRequestRepository
        {
            public readonly IMapper _mapper;
            private ApplicationDbContext _context;

            public RequestRepository(IMapper mapper, ApplicationDbContext context)
            {
                _mapper = mapper;
                _context = context;
            }

            public IEnumerable<Request> GetRequestList()
            {
                return _context.Request.ToList();
            }

            public async Task<CRUDRequestResponse> Create(Request request)
            {
                await _context.Request.AddAsync(request);
                var success = await _context.SaveChangesAsync();
                return new CRUDRequestResponse (request.Id, success > 0, null);
            }

            public async Task<CRUDRequestResponse> Update(Request request)
            {
                var success = await _context.SaveChangesAsync();
                return new CRUDRequestResponse(request.Id, success > 0, null);
            }

            public async Task<CRUDRequestResponse> Delete(Request request)
            {
                var success = await _context.SaveChangesAsync();
                return new CRUDRequestResponse(request.Id, success > 0, null);
            }
        }
    }
}