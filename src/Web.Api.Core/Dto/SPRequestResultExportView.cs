using System;
using System.Collections.Generic;
using System.Text;

namespace Web.Api.Core.Domain.Entities
{
    public partial class SPRequestResultExportView
    {
        public SPRequestResultExportView(string title, DateTime? startDate, DateTime? endDate, DateTime createdAt, string createdUserEmail, string requestStatus,
            DateTime? updatedAt,string updatedUserEmail, string serverIP, string serverName, string approvedUserEmail, string response, string description)
        {
            Title = title;
            StartDate = startDate;
            EndDate = endDate;
            Description = description;
            RequestStatus = requestStatus;
            Response = response;
            CreatedAt = createdAt;
            UpdatedAt = updatedAt;
            ServerName = serverName;
            ServerIP = serverIP;
            ApprovedUserEmail = approvedUserEmail;
            CreatedUserEmail = createdUserEmail;
            UpdatedUserEmail = updatedUserEmail;
        }

        
        public string Title { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedUserEmail { get; set; }
        public string RequestStatus { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string UpdatedUserEmail { get; set; }
        public string ServerName { get; set; }
        public string ServerIP { get; set; }
        public string ApprovedUserEmail { get; set; }
        public string Response { get; set; }
        public string Description { get; set; }

    }
}
