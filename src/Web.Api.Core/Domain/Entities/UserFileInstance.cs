using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Web.Api.Core.Domain.Entities
{
    public partial class UserFileInstance
    {
        public UserFileInstance(Guid userId, Guid fileInstanceId)
        {
            UserId = userId;
            FileInstanceId = fileInstanceId;
        }
        [Key]
        public Guid UserId { get; set; }
        public Guid FileInstanceId { get; set; }
        public virtual FileInstance FileInstance { get; set; }
        public virtual User User { get; set; }
    }
}
