using System;
using System.Collections.Generic;

namespace Web.Api.Core.Domain.Entities
{
    public partial class CustomerServer
    {
        public CustomerServer(Guid customerId, Guid serverId)
        {
            CustomerId = customerId;
            ServerId = serverId;
        }

        public Guid CustomerId { get; set; }
        public Guid ServerId { get; set; }
        public virtual Customer Customer { get; set; }
        public virtual Server Server { get; set; }
    }
}
