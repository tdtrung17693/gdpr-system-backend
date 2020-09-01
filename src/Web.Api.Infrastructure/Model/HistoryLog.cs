using System;
using System.Collections.Generic;

namespace Web.Api.Infrastructure.Model
{
    public partial class HistoryLog
    {
        public Guid Id { get; set; }
        public Guid RequestId { get; set; }
        public DateTime? CreatedAt { get; set; }
        public Guid CreatedBy { get; set; }
        public string UpdatedField { get; set; }
        public string UpdatedState { get; set; }
        public string PreviousState { get; set; }
        public string Message { get; set; }

        public virtual Request Request { get; set; }
    }
}
