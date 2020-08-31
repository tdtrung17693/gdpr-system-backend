using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Web.Api.Infrastructure.Data.EntityFramework.Entities
{
    public class Permission    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public ICollection<PermissionRole> PermissionRoles { get; set; }
    }
}