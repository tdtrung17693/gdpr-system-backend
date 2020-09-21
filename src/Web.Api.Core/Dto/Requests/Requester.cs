using System;

namespace Web.Api.Core.Dto.Requests
{
    public class Requester
    {
        public Guid UserId { get; set; }
        public string FullName { get; set;  }
        public string Username { get; set; }
        public string Email { get; set; }
    }
}
