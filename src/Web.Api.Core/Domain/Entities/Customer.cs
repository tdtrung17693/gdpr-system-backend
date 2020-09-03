using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Web.Api.Core.Domain.Entities
{
    public partial class Customer : BaseEntity
    {
        public Customer(string name,
          DateTime? contractBeginDate, DateTime? contractEndDate, Guid? contactPoint, string description,
          Guid? id, Guid? createdBy, DateTime? createdAt, Guid? updatedBy, DateTime? updatedAt, bool? status)
            : base(id, createdAt, createdBy, updatedAt, updatedBy, status: status)
        {
            Name = name;
            ContractBeginDate = contractBeginDate;
            ContractEndDate = contractEndDate;
            ContactPoint = contactPoint;
            Description = description;
        }
        public string Name { get; set; }
        public DateTime? ContractBeginDate { get; set; }
        public DateTime? ContractEndDate { get; set; }
        public Guid? ContactPoint { get; set; }
        public string Description { get; set; }
        public virtual ICollection<CustomerServer> CustomerServer { get; set; }
        public bool HasContactPoint(User user) {
            return user.Id == ContactPoint;
        }
    }
}
