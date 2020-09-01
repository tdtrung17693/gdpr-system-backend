using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Web.Api.Core.Domain.Entities
{
    public partial class Request
    {
        public Request(Guid id, Guid? createdBy, DateTime? createdAt, Guid? updatedBy, DateTime? updatedAt, Guid? deletedBy, DateTime? deletedAt, bool? isDeleted, 
            string status, string title, string description, DateTime startDate, DateTime endDate, Guid? serverId, string response, Guid? approvedBy)
        {
            Id = id;
            CreatedBy = createdBy;
            CreatedAt = createdAt;
            UpdatedBy = updatedBy;
            UpdatedAt = updatedAt;
            DeletedBy = deletedBy;
            DeletedAt = deletedAt;
            IsDeleted = isDeleted;
            Status = status;
            Title = title;
            Description = description;
            StartDate = startDate;
            EndDate = endDate;
            ServerId = serverId;
            Response = response;
            ApprovedBy = approvedBy;
        }

        /*public Request()
{
   Comment = new HashSet<Comment>();
   HistoryLog = new HashSet<HistoryLog>();
}*/
        [Key]
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
