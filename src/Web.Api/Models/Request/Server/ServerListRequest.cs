
using System;
using Web.Api.Core.Domain.Entities;

namespace Web.Api.Models.Request
{
    public class ServerListRequest
    {
        public Guid? id { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime? CreatedAt { get; set; }
        public Guid? UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public Guid? DeletedBy { get; set; }
        public DateTime? DeletedAt { get; set; }
        public bool? IsDeleted { get; set; }
        public string Name { get; set; }
        public string IpAddress { get; set; }
        public string? StartDate { get; set; }
        public string? EndDate { get; set; }
        public bool? Status { get; set; }
    }
}
