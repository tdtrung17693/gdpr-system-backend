using System;
using System.Collections.Generic;
using System.Text;

namespace Web.Api.Core.Domain.Entities
{
    public partial class ExportRequestDetail
    {
        public ExportRequestDetail(string title, string startDate,string endDate, string createdAt, string createdUserEmail, string requestStatus,
           string updatedAt, string updatedUserEmail, string serverIP, string serverName, string approvedUserEmail, string response, string description)
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
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string CreatedAt { get; set; }
        public string CreatedUserEmail { get; set; }
        public string RequestStatus { get; set; }
        public string UpdatedAt { get; set; }
        public string UpdatedUserEmail { get; set; }
        public string ServerName { get; set; }
        public string ServerIP { get; set; }
        public string ApprovedUserEmail { get; set; }
        public string Response { get; set; }
        public string Description { get; set; }

    }
}
