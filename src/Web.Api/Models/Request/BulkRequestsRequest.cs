using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Api.Models.Request
{
    public class BulkRequestsRequest
    {
        public Guid updator { get; set; }
        public bool requestStatus { get; set; }
        public IEnumerable<Guid> requestIdList { get; set; }

    }
}
