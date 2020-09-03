using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Web.Api.Auth.Requirements
{
  internal class PermissionAuthorizeAttribute : AuthorizeAttribute
  {
    const string POLICY_PREFIX = "Can";

    public PermissionAuthorizeAttribute(string permissionName) => Permission = permissionName;

    // Get or set the Age property by manipulating the underlying Policy property
    public string Permission
    {
      get
      {
        var tokens = Regex.Split(Policy.Substring(POLICY_PREFIX.Length), "(?<=[A-Z])(?=[A-Z][a-z])|(?<=[^A-Z])(?=[A-Z])|(?<=[A-Za-z])(?=[^A-Za-z])");

        return $"{tokens[1].ToLower()}:{tokens[0].ToLower()}";
      }
      set
      {
        var tokens = value.Split(":");
        Policy = $"{POLICY_PREFIX}{tokens[1].Substring(0, 1).ToUpper() + tokens[1].Substring(1).ToLower()}{tokens[0].Substring(0, 1).ToUpper() + tokens[0].Substring(1).ToLower()}";
      }
    }
  }
}
