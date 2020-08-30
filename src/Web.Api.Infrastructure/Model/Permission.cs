using System;
using System.Collections.Generic;

namespace Web.Api.Infrastructure.Model
{
    public partial class Permission
    {
        public Permission()
        {
            PermissionRole = new HashSet<PermissionRole>();
        }

        public Guid Id { get; set; }
        public string PermissionName { get; set; }

        public virtual ICollection<PermissionRole> PermissionRole { get; set; }
    }
}
