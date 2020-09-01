using Autofac.Core.Activators.Delegate;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Web.Api.Core.Domain.Entities
{
    public partial class Comment
    {
        public Comment(Guid id, Guid? createdBy, DateTime? createdAt, Guid? updatedBy, DateTime? updatedAt, Guid? deletedBy, DateTime? deletedAt, bool? isDeleted, Guid? requestId,
            Guid? parentId, string content)
        {
            Id = id;
            CreatedBy = createdBy;
            CreatedAt = createdAt;
            UpdatedBy = updatedBy;
            UpdatedAt = updatedAt;
            DeletedBy = deletedBy;
            DeletedAt = deletedAt;
            IsDeleted = isDeleted;
            RequestId = requestId;
            ParentId = parentId;
            Content = content;
        }
        [Key]
        public Guid Id { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime? CreatedAt { get; set; }
        public Guid? UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public Guid? DeletedBy { get; set; }
        public DateTime? DeletedAt { get; set; }
        public bool? IsDeleted { get; set; }
        public Guid? RequestId { get; set; }
        public Guid? ParentId { get; set; }
        public string Content { get; set; }

        public virtual User CreatedByNavigation { get; set; }
        public virtual User DeletedByNavigation { get; set; }
        public virtual Comment Parent { get; set; }
        public virtual Request Request { get; set; }
        public virtual User UpdatedByNavigation { get; set; }
        public virtual ICollection<Comment> InverseParent { get; set; }
        /*public virtual User CreatedByNavigation { get; set; }
        public virtual User DeletedByNavigation { get; set; }
        public virtual Comment Parent { get; set; }
        public virtual Request Request { get; set; }
        public virtual User UpdatedByNavigation { get; set; }
        public virtual ICollection<Comment> InverseParent { get; set; }*/
    }
}
