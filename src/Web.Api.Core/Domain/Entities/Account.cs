using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Web.Api.Core.Domain.Entities
{
    public partial class Account
    {
        [Key]
        public Guid Id { get; set; }
        public string Username { get; set; }
        public byte[] HashedPassword { get; set; }
        public string Salt { get; set; }
        public Guid UserId { get; set; }

        public virtual User User { get; set; }
        //public virtual User User { get; set; }
        public Account(Guid id, string username, byte[] hashedPassword, string salt, Guid userId)
        {
            Id = id;
            Username = username;
            HashedPassword = hashedPassword;
            Salt = salt;
            UserId = userId;
        }
    }
}
