using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;

namespace Web.Api.Core.Domain.Entities
{
    public partial class Request : BaseEntity
    {
        public Request(string title, DateTime startDate, DateTime endDate, Guid? serverId, [Optional] string description, string requestStatus, [Optional] string response, [Optional] Guid? approvedBy, Guid? id, Guid? createdBy, DateTime? createdAt, [Optional] Guid? updatedBy, [Optional] DateTime? updatedAt, [Optional] Guid? deletedBy, [Optional] DateTime? deletedAt)
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
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public Guid? ServerId { get; set; }
        public string Response { get; set; }
        public Guid? ApprovedBy { get; set; }
        //public virtual User ApprovedByNavigation { get; set; }
        public virtual Server Server { get; set; }
        public virtual ICollection<Comment> Comment { get; set; }
        public virtual ICollection<HistoryLog> HistoryLog { get; set; }
    }
}
