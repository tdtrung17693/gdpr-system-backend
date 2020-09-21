using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;

namespace Web.Api.Core.Domain.Entities
{
    public partial class Request : BaseEntity
    {
        public Request(string title, DateTime? startDate, DateTime? endDate, Guid serverId, string description, string requestStatus, string response, Guid? approvedBy, Guid? id, Guid? createdBy, DateTime? createdAt,  Guid? updatedBy, DateTime? updatedAt, Guid? deletedBy, DateTime? deletedAt)
            : base(id, createdAt, createdBy, updatedAt, updatedBy, deletedAt, deletedBy)
        {
            Title = title;
            Description = description;
            StartDate = startDate;
            EndDate = endDate;
            ServerId = serverId;
            Response = response;
            ApprovedBy = approvedBy;
            RequestStatus = requestStatus;
        }

        public string RequestStatus { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public Guid ServerId { get; set; }
        public string Response { get; set; }
        public Guid? ApprovedBy { get; set; }
        public virtual User ApprovedByNavigation { get; set; }
        public virtual User CreatedByNavigation { get; set; }
        public virtual Server Server { get; set; }
        public virtual ICollection<Comment> Comment { get; set; }
        public virtual ICollection<HistoryLog> HistoryLog { get; set; }
    }
}
