using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Api.Models.Request
{
    public class ManageRequestRequestModel
    {
        public string userId { get; set; }
        public string answer { get; set; }
        public string status { get; set; }
        public string requestId { get; set; }
    }
}
