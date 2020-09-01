﻿using System;
using System.Collections.Generic;

namespace Web.Api.Infrastructure.Model
{
    public partial class PermissionRole
    {
        public Guid RoleId { get; set; }
        public Guid PermissionId { get; set; }

        public virtual Permission Permission { get; set; }
        public virtual Role Role { get; set; }
    }
}
