using System;
using System.Collections.Generic;

namespace Web.Api.Infrastructure.Model
{
    public partial class Role
    {
        public Role()
        {
            PermissionRole = new HashSet<PermissionRole>();
            User = new HashSet<User>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<PermissionRole> PermissionRole { get; set; }
        public virtual ICollection<User> User { get; set; }
    }
}
