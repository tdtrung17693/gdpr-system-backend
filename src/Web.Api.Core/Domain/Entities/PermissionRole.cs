using System;
using System.Collections.Generic;

namespace Web.Api.Core.Domain.Entities
{
    public partial class PermissionRole
    {
        public PermissionRole(Guid roleId, Guid permissionId)
        {
            RoleId = roleId;
            PermissionId = permissionId;
        }

        public Guid RoleId { get; set; }
        public Guid PermissionId { get; set; }

        public virtual Permission Permission { get; set; }
        public virtual Role Role { get; set; }
    }
}
