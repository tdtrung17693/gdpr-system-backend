using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Api.Core.Domain.Entities;
using Web.Api.Core.Interfaces.Gateways.Repositories;

namespace Web.Api.Auth
{
    public class AuthService
    {
        private User _user = null;
        private IEnumerable<string> _permissions;
        private readonly IUserRepository _userRepository;
        private readonly IPermissionRepository _permissionRepository;

        public AuthService(IUserRepository userRepository, IPermissionRepository permissionRepository)
        {
            _userRepository = userRepository;
            _permissionRepository = permissionRepository;
            _permissions = new List<string>();
        }

        public async Task LogIn(string userId)
        {
            _user = await _userRepository.FindById(userId);
            var permissions = await _permissionRepository.GetPermissionsOfRole(_user.RoleId.ToString());
            _permissions = permissions.Select(p => p.PermissionName);
        }

        public bool HasPermission(string requiredPermission)
        {
            return _permissions.Contains(requiredPermission);
        }
    }
}