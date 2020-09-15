using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Web.Api.Core.Domain.Entities
{
    public class Server : BaseEntity
    {
        public Server(Guid? id, DateTime? createdAt, Guid? createdBy, DateTime? deletedAt, Guid? deletedBy
            , DateTime? endDate, string ipAddress, bool? isDeleted, string name, DateTime? startDate, bool? status, DateTime? updatedAt, Guid? updatedBy)
            : base(id, createdAt, createdBy, updatedAt, updatedBy, deletedAt, deletedBy, isDeleted, status)
        {
            Id = id;
            Name = name;
            IpAddress = ipAddress;
            StartDate = startDate;
            EndDate = endDate;
            Request = new HashSet<Request>();
        }
        public string Name { get; set; }
        public string IpAddress { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public virtual ICollection<CustomerServer> CustomerServer { get; set; }
        public virtual ICollection<Request> Request { get; set; }
    }
}
