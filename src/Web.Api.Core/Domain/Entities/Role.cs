using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Web.Api.Core.Domain.Entities
{
    public partial class Role
    {
        public Role(Guid id, string name)
        {
            Id = id;
            Name = name;
        }

        /*public Role()
{
   PermissionRole = new HashSet<PermissionRole>();
   User = new HashSet<User>();
}*/
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<PermissionRole> PermissionRole { get; set; }
        public virtual ICollection<User> User { get; set; }
    }
}
