using System;
using System.Collections.Generic;

namespace Web.Api.Infrastructure.Model
{
    public partial class CustomerServer
    {
        public Guid CustomerId { get; set; }
        public Guid ServerId { get; set; }

        public virtual Customer Customer { get; set; }
        public virtual Server Server { get; set; }
    }
}
