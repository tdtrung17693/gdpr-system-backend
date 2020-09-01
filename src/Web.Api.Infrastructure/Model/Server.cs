using System;
using System.Collections.Generic;

namespace Web.Api.Infrastructure.Model
{
    public partial class Server
    {
        public Server()
        {
            CustomerServer = new HashSet<CustomerServer>();
            Request = new HashSet<Request>();
        }

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
