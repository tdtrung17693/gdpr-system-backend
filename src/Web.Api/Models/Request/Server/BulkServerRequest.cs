
using System;
using System.Collections.Generic;
using Web.Api.Core.Domain.Entities;

namespace Web.Api.Models.Request
{
    public class BulkServerRequest
    {
        public Guid updator { get; set; }
        public bool status { get; set; }
        public IEnumerable<Guid> serverIdList { get; set; }
       
    }
}
