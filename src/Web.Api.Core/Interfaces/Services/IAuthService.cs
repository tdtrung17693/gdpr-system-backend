using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Web.Api.Core.Domain.Entities;

namespace Web.Api.Core.Interfaces.Services
{
  public interface IAuthService
  {
    Task<bool> LogIn(string userId);
    bool HasPermission(string requiredPermission);
    User GetCurrentUser();
    IEnumerable<string> GetAllPermissions();
  }
}
