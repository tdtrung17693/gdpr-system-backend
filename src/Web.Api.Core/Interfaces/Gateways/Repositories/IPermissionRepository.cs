using System.Collections.Generic;
using System.Threading.Tasks;
using Web.Api.Core.Domain.Entities;

namespace Web.Api.Core.Interfaces.Gateways.Repositories
{
    public interface IPermissionRepository
    {
        Task<IEnumerable<Permission>> GetPermissionsOfRole(string roleId);
    }
}