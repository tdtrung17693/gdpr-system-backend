using System;

namespace Web.Api.Core.Domain.Entities
{
    public class RequestDetail : BaseEntity
    {
        public RequestDetail(string title, DateTime? startDate, DateTime? endDate, Guid serverId, string description, string requestStatus, string response, Guid? approvedBy, Guid id, Guid createdBy, DateTime createdAt, Guid? updatedBy, DateTime? updatedAt, Guid? deletedBy, DateTime? deletedAt,
                              string serverName, string serverIP, string createdByName, string updatedByName)
            : base(id, createdAt, createdBy, updatedAt, updatedBy, deletedAt, deletedBy)
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
        }
       
        public string RequestStatus { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public Guid? ServerId { get; set; }
        public string Response { get; set; }
        public Guid? ApprovedBy { get; set; }

        public string ServerName { get; set; }
        public string ServerIP { get; set; }
        
        public string CreatedByName { get; set; }
        
        public string UpdatedByName { get; set; }
    }
}
