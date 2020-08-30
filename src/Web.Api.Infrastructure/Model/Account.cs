using System;
using System.Collections.Generic;

namespace Web.Api.Infrastructure.Model
{
    public partial class Account
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public byte[] HashedPassword { get; set; }
        public string Salt { get; set; }
        public Guid? UserId { get; set; }

        public virtual User User { get; set; }
    }
}
