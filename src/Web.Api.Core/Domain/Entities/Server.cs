
using System;
using System.Dynamic;

namespace Web.Api.Core.Domain.Entities
{
    public class Server
    {
        public Guid Id { get; }
        public DateTime CreatedBy { get; }
        public DateTime CreatedAt { get; }
        public DateTime UpdatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime DeletedBy { get; set; }
        public DateTime DeletedAt { get; set; }
        public bool isDeleted { get; set; }
        public string Name { get; set; }
        public string IpAddress { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        internal Server(string Id, DateTime CreatedBy, DateTime CreatedAt, DateTime UpdatedBy, DateTime UpdatedAt, bool isDeleted, string Name, string IpAddress, DateTime StartDate, DateTime EndDate)
        {
            Id = Id;
            CreatedBy = CreatedBy;

            CreatedAt = CreatedAt;
            
            UpdatedBy = UpdatedBy;
            
            UpdatedAt = UpdatedAt;
            
            DeletedBy = DeletedBy;
            
            DeletedAt = DeletedAt;
            
            isDeleted = isDeleted;
            
            Name = Name;
            
            IpAddress = IpAddress;
            
            StartDate = StartDate;
            
            EndDate = EndDate;

        }
    }
}
