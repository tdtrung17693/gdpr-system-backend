using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace Web.Api.Core.Domain.Entities
{
    public partial class Customer : BaseEntity
    {
        public Customer(string name,
          DateTime? contractBeginDate, DateTime? contractEndDate, Guid? contactPoint, string description, bool? status = true,
          Guid? id = null, Guid? createdBy = null, DateTime? createdAt = null, Guid? updatedBy = null, DateTime? updatedAt = null)
            : base(id, createdAt, createdBy, updatedAt, updatedBy, status: status)
        {
            Name = name;
            ContractBeginDate = contractBeginDate;
            ContractEndDate = contractEndDate;
            ContactPoint = contactPoint;
            Description = description;
            CustomerServer = new HashSet<CustomerServer>();
        }
        public string Name { get; set; }
        public DateTime? ContractBeginDate { get; set; }
        public DateTime? ContractEndDate { get; set; }
        public Guid? ContactPoint { get; set; }
        public string Description { get; set; }
        public virtual ICollection<CustomerServer> CustomerServer { get; set; }
        public virtual User ContactPointNavigation { get; set; }
        //Unmapped Properties
        //public override bool IsDeleted { get; set; }
    }
}
