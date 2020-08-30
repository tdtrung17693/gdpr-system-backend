using System;
using System.Collections.Generic;

namespace Web.Api.Infrastructure.Model
{
    public partial class UserLog
    {
        public Guid Id { get; set; }
        public string Behavior { get; set; }
        public DateTime CreatedAt { get; set; }
        public Guid UserId { get; set; }

        public virtual User User { get; set; }
    }
}
