using Autofac.Core.Activators.Delegate;
using System;
using System.Collections.Generic;

namespace Web.Api.Core.Domain.Entities
{
    public partial class Comment : BaseEntity
    {
        public Comment(
            Guid requestId,
            Guid? parentId, string content,
            Guid? id, Guid? createdBy, DateTime? createdAt, Guid? updatedBy, DateTime? updatedAt, Guid? deletedBy, DateTime? deletedAt, bool? isDeleted, bool? status
        )
            : base(id, createdAt, createdBy, updatedAt, updatedBy, deletedAt, deletedBy, isDeleted, status)
        {
            RequestId = requestId;
            ParentId = parentId;
            Content = content;
        }
        public Guid RequestId { get; set; }
        public Guid? ParentId { get; set; }
        public string Content { get; set; }
        public virtual Comment Parent { get; set; }
        public virtual Request Request { get; set; }
        public virtual ICollection<Comment> InverseParent { get; set; }
        public virtual User Author { get; set; }
    }
}
