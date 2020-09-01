using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Web.Api.Core.Domain.Entities
{
    public partial class Permission
    {
        public Permission(Guid id, string permissionName)
        {
            Id = id;
            PermissionName = permissionName;
        }

        /*public Permission()
{
   PermissionRole = new HashSet<PermissionRole>();
}*/
        [Key]
        public Guid Id { get; set; }
        public string PermissionName { get; set; }

        public virtual ICollection<PermissionRole> PermissionRole { get; set; }
    }
}
