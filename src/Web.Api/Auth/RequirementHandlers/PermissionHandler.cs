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
        public PermissionHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
        {
            var uid = context.User.Claims.Single(claim => claim.Type == Constants.Strings.JwtClaimIdentifiers.Id).Value;
            var user = await _userRepository.FindById(uid);

            var permissions = user.GetPermissions();
            var requiredPermission = requirement.Permission;
            if (permissions.Contains(requiredPermission)) context.Succeed(requirement);
        }
    }
}
