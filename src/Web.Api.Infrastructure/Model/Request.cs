using System;
using System.Collections.Generic;

namespace Web.Api.Infrastructure.Model
{
    public partial class Request
    {
        public Request()
        {
            Comment = new HashSet<Comment>();
            HistoryLog = new HashSet<HistoryLog>();
        }

        public Guid Id { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime? CreatedAt { get; set; }
        public Guid? UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public Guid? DeletedBy { get; set; }
        public DateTime? DeletedAt { get; set; }
        public bool? IsDeleted { get; set; }
        public string Status { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public Guid? ServerId { get; set; }
        public string Response { get; set; }
        public Guid? ApprovedBy { get; set; }

        public virtual User ApprovedByNavigation { get; set; }
        public virtual User CreatedByNavigation { get; set; }
        public virtual User DeletedByNavigation { get; set; }
        public virtual Server Server { get; set; }
        public virtual User UpdatedByNavigation { get; set; }
        public virtual ICollection<Comment> Comment { get; set; }
        public virtual ICollection<HistoryLog> HistoryLog { get; set; }
    }
}
