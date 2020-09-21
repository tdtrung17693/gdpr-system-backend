using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Web.Api.Core.Domain.Entities;

namespace Web.Api.Core.Interfaces.Services
{
  public interface IAuthService
  {
    Task<bool> LogIn(Guid userId);
    bool HasPermission(string requiredPermission);
    User GetCurrentUser();
    Object GetCurrentUserAvatar();
    IEnumerable<string> GetAllPermissions();
  }
}
