using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Web.Api.Auth.Requirements;
using Web.Api.Core.Domain.Entities;
using Web.Api.Core.Interfaces.Gateways.Repositories;
using Web.Api.Infrastructure.Helpers;

namespace Web.Api.Auth.RequirementHandlers
{
  public class PermissionHandler : AuthorizationHandler<PermissionRequirement>
  {
    private IUserRepository _userRepository;
    private AuthService _authService;

    public PermissionHandler(IUserRepository userRepository, AuthService authService)
    {
      _userRepository = userRepository;
      _authService = authService;
    }
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
    {
      var IdClaim = Constants.Strings.JwtClaimIdentifiers.Id;
      if (context.User.HasClaim(c => c.Type == IdClaim))
      {
        var requiredPermission = requirement.Permission;
        if (_authService.HasPermission(requiredPermission)) context.Succeed(requirement);
      }

      return Task.CompletedTask;
    }
  }
}
