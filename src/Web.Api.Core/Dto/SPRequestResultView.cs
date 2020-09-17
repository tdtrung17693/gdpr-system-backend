using System;
using System.Collections.Generic;
using System.Text;

namespace Web.Api.Core.Domain.Entities
{
    public partial class SPRequestResultView
    {
        public SPRequestResultView(string title, DateTime? startDate, DateTime? endDate, Guid serverId, string description, string requestStatus, string response, Guid? approvedBy, Guid id, Guid createdBy, DateTime createdAt, Guid? updatedBy, DateTime? updatedAt, Guid? deletedBy, DateTime? deletedAt,
                              string serverName, string serverIP, string createdByFName, string createdByLName, string createdByEmail, string updatedByFName, string updatedByLName, string updatedByEmail)
        {
            Title = title;
            StartDate = startDate;
            EndDate = endDate;
            ServerId = serverId;
            Description = description;
            RequestStatus = requestStatus;
            Response = response;
            ApprovedBy = approvedBy;
            Id = id;
            CreatedAt = createdAt;
            CreatedBy = createdBy;
            UpdatedBy = updatedBy;
            UpdatedAt = updatedAt;
            UpdatedBy = updatedBy;
            DeletedAt = deletedAt;
            DeletedBy = deletedBy;

            ServerName = serverName;
            ServerIP = serverIP;
            CreatedByFName = createdByFName;
            CreatedByLName = createdByLName;
            CreatedByEmail = createdByEmail;
            UpdatedByFName = updatedByFName;
            UpdatedByLName = updatedByLName;
            UpdatedByEmail = updatedByEmail;
        }

        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; }

        public Guid CreatedBy { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public Guid? UpdatedBy { get; set; }

        public DateTime? DeletedAt { get; set; }

        public Guid? DeletedBy { get; set; }
        public string RequestStatus { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public Guid ServerId { get; set; }
        public string Response { get; set; }
        public Guid? ApprovedBy { get; set; }

        public string ServerName { get; set; }
        public string ServerIP { get; set; }

        public string CreatedByFName { get; set; }
        public string CreatedByLName { get; set; }
        public string CreatedByEmail { get; set; }
        public string UpdatedByFName { get; set; }
        public string UpdatedByLName { get; set; }
        public string UpdatedByEmail { get; set; }
    }
}
