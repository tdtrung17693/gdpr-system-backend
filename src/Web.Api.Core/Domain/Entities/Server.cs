
using System;
using System.Dynamic;

namespace Web.Api.Core.Domain.Entities
{
    public class Server
    {
        public Guid Id { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public Guid? UpdatedBy { get; set; }
        public Nullable<DateTime> UpdatedAt { get; set; }
        public Guid? DeletedBy { get; set; }
        public Nullable<DateTime> DeletedAt { get; set; }
        public bool isDeleted { get; set; }
        public string Name { get; set; }
        public string IpAddress { get; set; }
        public Nullable<DateTime> StartDate { get; set; }
        public Nullable<DateTime> EndDate { get; set; }

        internal Server(Guid Id, Guid CreatedBy, DateTime CreatedAt, Guid? UpdatedBy, Nullable<DateTime> UpdatedAt, Guid? DeletedBy, Nullable<DateTime> DeletedAt, bool isDeleted, string Name, string IpAddress, Nullable<DateTime> StartDate, Nullable<DateTime> EndDate)
        {
            Id = Id;

            CreatedBy = CreatedBy;

            CreatedAt = CreatedAt;

            UpdatedBy = UpdatedBy == null ? Guid.Empty : UpdatedBy;
            
            UpdatedAt = UpdatedAt;

            DeletedBy = DeletedBy == null ? Guid.Empty : DeletedBy;
            
            DeletedAt = DeletedAt;
            
            isDeleted = isDeleted;
            
            Name = Name;
            
            IpAddress = IpAddress;
            
            StartDate = StartDate;
            
            EndDate = EndDate;

        }
    }
}
