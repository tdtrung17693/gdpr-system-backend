using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Api.Models.Request
{
    public class ExportRequestModel
    {
        public DateTime fromDate { get; set; }
        public DateTime toDate { get; set; }
        public ICollection<Guid?> guids { get; set; }
    }
}
