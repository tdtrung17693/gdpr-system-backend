using System;
using System.Collections.Generic;

namespace Web.Api.Infrastructure.Model
{
    public partial class EmailLog
    {
        public Guid Id { get; set; }
        public string Receiver { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string Status { get; set; }
    }
}
