
using System;
using Web.Api.Core.Domain.Entities;

namespace Web.Api.Models.Request
{
    public class ServerImportRequest
    {
        public Guid CreatedBy { get; set; }
        public string Name { get; set; }
        public string IpAddress { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
