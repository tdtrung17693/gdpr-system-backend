
using System;

namespace Web.Api.Models.Request
{
    public class ServerRequest
    {
        public string Name { get; set; }
        public string IpAddress { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
