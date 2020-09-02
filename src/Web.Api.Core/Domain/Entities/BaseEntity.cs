using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Web.Api.Core.Domain.Entities
{
    public class BaseEntity
    {
        public BaseEntity(Guid? id, DateTime? createdAt, Guid? createdBy, DateTime? updatedAt, Guid? updatedBy, DateTime? deletedAt = null, Guid? deletedBy = null, bool? isDeleted = false, bool? status = true)
        {
            Id = id;
            IsDeleted = isDeleted;
            Status = status;
            CreatedBy = createdBy;
            CreatedAt = createdAt;
            UpdatedBy = updatedBy;
            UpdatedAt = updatedAt;
            DeletedBy = deletedBy;
            DeletedAt = deletedAt;
        }

        [Key]
        public Guid? Id { get; set; }
        public DateTime? CreatedAt { get; set; }

        public Guid? CreatedBy { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public Guid? UpdatedBy { get; set; }

        public DateTime? DeletedAt { get; set; }

        public Guid? DeletedBy { get; set; }

        public bool? Status { get; set; }
        public bool? IsDeleted { get; set; }
        public virtual User CreatedByNavigation { get; set; }
        public virtual User DeletedByNavigation { get; set; }
        public virtual User UpdatedByNavigation { get; set; }
    }
}
