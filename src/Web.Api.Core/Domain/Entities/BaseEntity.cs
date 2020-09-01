using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Web.Api.Core.Domain.Entities
{
    public class BaseEntity
    {
        public BaseEntity()
        {
            Status = true;
            IsDeleted = false;
        }

        [Key]
        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; }

        public Guid? CreatedBy { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public Guid? UpdatedBy { get; set; }

        public DateTime? DeletedAt { get; set; }

        public Guid? DeletedBy { get; set; }

        public bool Status { get; set; }
        public bool IsDeleted { get; set; }
    }
}
