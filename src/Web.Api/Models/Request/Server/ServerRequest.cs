
using System;
using Web.Api.Core.Domain.Entities;

namespace Web.Api.Models.Request
{
    public class ServerRequest
    {
        public Guid Id { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public Guid? UpdatedBy { get; set; }
        public Nullable<DateTime> UpdatedAt { get; set; }
        public Guid? DeletedBy { get; set; }
        public Nullable<DateTime> DeletedAt { get; set; }
        public bool IsDeleted { get; set; }
        public string Name { get; set; }
        public string IpAddress { get; set; }
        public Nullable<DateTime> StartDate { get; set; }
        public Nullable<DateTime> EndDate { get; set; }
        public bool? Status { get; set; }
    }
}
