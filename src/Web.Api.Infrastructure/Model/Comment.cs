using System;
using System.Collections.Generic;

namespace Web.Api.Infrastructure.Model
{
    public partial class Comment
    {
        public Comment()
        {
            InverseParent = new HashSet<Comment>();
        }

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
    }
}
