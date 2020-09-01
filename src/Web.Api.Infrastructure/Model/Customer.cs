using System;
using System.Collections.Generic;

namespace Web.Api.Infrastructure.Model
{
    public partial class Customer
    {
        public Customer()
        {
            CustomerServer = new HashSet<CustomerServer>();
        }

        public Guid Id { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime? CreatedAt { get; set; }
        public Guid? UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string Name { get; set; }
        public DateTime? ContractBeginDate { get; set; }
        public DateTime? ContractEndDate { get; set; }
        public Guid? ContactPoint { get; set; }
        public string Description { get; set; }
        public bool? Status { get; set; }

        public virtual User CreatedByNavigation { get; set; }
        public virtual User UpdatedByNavigation { get; set; }
        public virtual ICollection<CustomerServer> CustomerServer { get; set; }
    }
}
