using System;
using System.Collections.Generic;
using System.Text;

namespace Web.Api.Infrastructure.Data.EntityFramework.Entities
{
    public class PermissionRole
    {
        public Guid PermissionId { get; set; }
        public Permission Permission { get; set; }
        public Guid RoleId { get; set; }
        public Role Role { get; set; }
    }
}
