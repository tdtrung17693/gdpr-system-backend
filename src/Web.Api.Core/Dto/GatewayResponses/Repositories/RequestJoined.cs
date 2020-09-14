using System;
using System.Collections.Generic;
using System.Text;

namespace Web.Api.Core.Dto.GatewayResponses.Repositories
{
    public class RequestJoined
    {
        public Guid Id { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public Guid? UpdatedBy { get; set; }
        public Nullable<DateTime> UpdatedAt { get; set; }
        public Guid? DeletedBy { get; set; }
        public Nullable<DateTime> DeletedAt { get; set; }
        public string RequestStatus { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public Guid? ServerId { get; set; }
        public string Response { get; set; }
        public Guid? ApprovedBy { get; set; }
        public string serverName { get; set; }
        public string userFName { get; set; }
        public string userLName { get; set; }
    }
}
