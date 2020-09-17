using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Web.Api.Core.Domain.Entities;
using Web.Api.Core.Interfaces.Gateways.Repositories;

namespace Web.Api.Infrastructure.Data.EntityFramework.Repositories
{
    public class PermissionRepository : IPermissionRepository
    {
        private ApplicationDbContext _context;
        public PermissionRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        
        public async Task<IEnumerable<Permission>> GetPermissionsOfRole(string roleId)
        {
            return await _context.PermissionRole
                .Include(pr => pr.Permission)
                .Where(pr => pr.RoleId.ToString() == roleId)
                .Select(pr => pr.Permission)
                .ToListAsync();
        }
    }
}