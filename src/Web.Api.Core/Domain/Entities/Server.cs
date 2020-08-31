
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
            this.Id = Id;

            this.CreatedBy = CreatedBy;

            this.CreatedAt = CreatedAt;

            this.UpdatedBy = UpdatedBy == null ? Guid.Empty : UpdatedBy;
            
            this.UpdatedAt = UpdatedAt;

            this.DeletedBy = DeletedBy == null ? Guid.Empty : DeletedBy;
            
            this.DeletedAt = DeletedAt;
            
            this.isDeleted = isDeleted;
            
            this.Name = Name;
            
            this.IpAddress = IpAddress;
            
            this.StartDate = StartDate;
            
            this.EndDate = EndDate;

        }
    }
}