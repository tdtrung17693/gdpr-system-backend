using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Web.Api.Core.Domain.Entities
{
    public partial class UserLog
    {
        public UserLog(Guid id, string behavior, DateTime createdAt, Guid userId)
        {
            Id = id;
            Behavior = behavior;
            CreatedAt = createdAt;
            UserId = userId;
        }
        [Key]
        public Guid Id { get; set; }
        public string Behavior { get; set; }
        public DateTime CreatedAt { get; set; }
        public Guid UserId { get; set; }
        public virtual User User { get; set; }

    }
}
