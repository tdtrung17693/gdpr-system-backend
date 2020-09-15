using System;

namespace Web.Api.Core.Dto.Requests
{
    public class NewRequestData
    {
        public string Username { get; set; }
        public string ServerName { get; set; }
        public Guid ServerId { get; set; }
        public Guid RequestId { get; set; }
    }
}