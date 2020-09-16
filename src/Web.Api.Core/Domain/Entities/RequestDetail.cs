using System;

namespace Web.Api.Core.Domain.Entities
{
    public class RequestDetail 
    {
        public RequestDetail(string title, string startDate, string endDate, Guid serverId, string description, string requestStatus, string response, Guid? approvedBy, Guid id, Guid createdBy, string createdAt, Guid? updatedBy, string updatedAt, Guid? deletedBy, string deletedAt,
                              string serverName, string serverIP, string createdByName, string createdByEmail, string createdByNameEmail, string updatedByName, string updatedByEmail, string updatedByNameEmail)
            
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
            UpdatedBy = updatedBy;
            UpdatedAt = updatedAt;
            UpdatedBy = updatedBy;
            DeletedAt = deletedAt;
            DeletedBy = deletedBy;
            
            ServerName = serverName;
            ServerIP = serverIP;
            CreatedByName = createdByName;
            UpdatedByName = updatedByName;
            CreatedByEmail = createdByEmail;
            UpdatedByEmail = updatedByEmail;
            CreatedByNameEmail = createdByNameEmail;
            UpdatedByNameEmail = updatedByNameEmail;
        }

        public Guid Id { get; set; }
        public string CreatedAt { get; set; }

        public Guid CreatedBy { get; set; }

        public string UpdatedAt { get; set; }

        public Guid? UpdatedBy { get; set; }

        public string DeletedAt { get; set; }

        public Guid? DeletedBy { get; set; }

        public bool? Status { get; set; }
        public bool? IsDeleted { get; set; }

        public string RequestStatus { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public Guid? ServerId { get; set; }
        public string Response { get; set; }
        public Guid? ApprovedBy { get; set; }

        public string ServerName { get; set; }
        public string ServerIP { get; set; }

        public string CreatedByName { get; set; }
        public string CreatedByEmail { get; set; }
        public string CreatedByNameEmail { get; set; }
        public string UpdatedByName { get; set; }
        public string UpdatedByEmail { get; set; }
        public string UpdatedByNameEmail { get; set; }

    }
}
