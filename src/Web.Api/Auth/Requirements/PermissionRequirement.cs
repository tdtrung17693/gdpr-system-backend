using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Api.Auth.Requirements
{
  public class PermissionRequirement : IAuthorizationRequirement
  {
    public PermissionRequirement(string permissionName)
    {
      this.Permission = permissionName;
    }

    public string Permission { get; }
  }
}
