using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Web.Api.Core.Domain.Entities
{
    public partial class EmailLog
    {
        public EmailLog(Guid id, string receiver, string subject, string content, DateTime? createdAt, string status)
        {
            Id = id;
            Receiver = receiver;
            Subject = subject;
            Content = content;
            CreatedAt = createdAt;
            Status = status;
        }
        [Key]
        public Guid Id { get; set; }
        public string Receiver { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string Status { get; set; }


    }
}
