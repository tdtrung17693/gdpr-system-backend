using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Web.Api.Infrastructure.Data.EntityFramework.Entities
{
    public class Account
    {
        [Key]
        public Guid Id { get; set; }
        public string Username { get; set; }
        public byte[] HashedPassword { get; set; }
        public string Salt { get; set; }

        public virtual User User { get; set; }
        public Guid UserId { get; set; }
    }
}
