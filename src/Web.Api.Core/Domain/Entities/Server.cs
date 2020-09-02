using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Web.Api.Core.Domain.Entities
{
    public partial class Server : BaseEntity
    {
        public Server(Guid? id, DateTime? createdAt, Guid? createdBy, DateTime? updatedAt, Guid? updatedBy, DateTime? deletedAt, Guid? deletedBy,  bool? isDeleted, bool? status, string name, 
            string ipAddress, DateTime? startDate, DateTime? endDate)
            : base(id, createdAt, createdBy, updatedAt, updatedBy, deletedAt, deletedBy, isDeleted, status)
        {
            Name = name;
            IpAddress = ipAddress;
            StartDate = startDate;
            EndDate = endDate;
        }

        public string Name { get; set; }
        public string IpAddress { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public virtual ICollection<CustomerServer> CustomerServer { get; set; }
        public virtual ICollection<Request> Request { get; set; }
    }
}
