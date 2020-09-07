using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Web.Api.Auth.Requirements;

namespace Web.Api.Auth
{
  internal class HavePermissionProvider : IAuthorizationPolicyProvider
  {
    const string POLICY_PREFIX = "Can";

    public Task<AuthorizationPolicy> GetDefaultPolicyAsync()
    {
          return Task.FromResult(new AuthorizationPolicyBuilder(JwtBearerDefaults.AuthenticationScheme).RequireAuthenticatedUser().Build());
    }

    // Policies are looked up by string name, so expect 'parameters' (like age)
    // to be embedded in the policy names. This is abstracted away from developers
    // by the more strongly-typed attributes derived from AuthorizeAttribute
    // (like [MinimumAgeAuthorize()] in this sample)
    public Task<AuthorizationPolicy> GetPolicyAsync(string policyName)
    {
      // Policy name has format of: CanViewUserAndEditUser, CanViewRequest, etc.
      if (policyName.StartsWith(POLICY_PREFIX, StringComparison.OrdinalIgnoreCase))
      {
        var permission = policyName.Substring(POLICY_PREFIX.Length);
        var policy = new AuthorizationPolicyBuilder(JwtBearerDefaults.AuthenticationScheme);

        // splitting pascal case permission
        var tokens = Regex.Split(permission, "(?<=[A-Z])(?=[A-Z][a-z])|(?<=[^A-Z])(?=[A-Z])|(?<=[A-Za-z])(?=[^A-Za-z])");
        // permission name has format: "user:view", "user:edit"
        var permissionName = $"{tokens[1].ToLower()}:{tokens[0].ToLower()}";

        policy.AddRequirements(new PermissionRequirement(permissionName));
        return Task.FromResult(policy.Build());
      }

      return Task.FromResult<AuthorizationPolicy>(null);
    }
  }
}
