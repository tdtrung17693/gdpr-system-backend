using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Api.Core.Domain.Entities;
using Web.Api.Core.Interfaces.Gateways.Repositories;

namespace Web.Api.Infrastructure.Data.EntityFramework.Repositories
{
  public class RoleRepository : IRoleRepository
  {
    private readonly ApplicationDbContext _context;
    public RoleRepository(ApplicationDbContext context)
    {
      _context = context;
    }

    public async Task<IEnumerable<Role>> FindAll()
    {
      return await _context.Role.ToListAsync();
    }

    public async Task<bool> IsExisted(Guid Id)
    {
      return (await _context.Role.Where(r => r.Id == Id).CountAsync()) > 0;
    }
  }
}
