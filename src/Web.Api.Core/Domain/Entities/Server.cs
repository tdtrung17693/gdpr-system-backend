using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Web.Api.Core.Domain.Entities
{
    public partial class Server
    {
        public Server(Guid id, Guid? createdBy, DateTime? createdAt, Guid? updatedBy, DateTime? updatedAt, Guid? deletedBy, DateTime? deletedAt, bool? isDeleted, string name, 
            string ipAddress, DateTime? startDate, DateTime? endDate)
        {
            Id = id;
            CreatedBy = createdBy;
            CreatedAt = createdAt;
            UpdatedBy = updatedBy;
            UpdatedAt = updatedAt;
            DeletedBy = deletedBy;
            DeletedAt = deletedAt;
            IsDeleted = isDeleted;
            Name = name;
            IpAddress = ipAddress;
            StartDate = startDate;
            EndDate = endDate;
        }

        /*public Server()
{
   CustomerServer = new HashSet<CustomerServer>();
   Request = new HashSet<Request>();
}*/
        [Key]
        public Guid Id { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime? CreatedAt { get; set; }
        public Guid? UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public Guid? DeletedBy { get; set; }
        public DateTime? DeletedAt { get; set; }
        public bool? IsDeleted { get; set; }
        public string Name { get; set; }
        public string IpAddress { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public virtual ICollection<CustomerServer> CustomerServer { get; set; }
        public virtual ICollection<Request> Request { get; set; }
    }
}
