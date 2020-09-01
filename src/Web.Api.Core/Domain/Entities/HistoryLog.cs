using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Web.Api.Core.Domain.Entities
{
    public partial class HistoryLog
    {
        public HistoryLog(Guid id, Guid requestId, DateTime? createdAt, Guid createdBy, string updatedField, string updatedState, string previousState, string message)
        {
            Id = id;
            RequestId = requestId;
            CreatedAt = createdAt;
            CreatedBy = createdBy;
            UpdatedField = updatedField;
            UpdatedState = updatedState;
            PreviousState = previousState;
            Message = message;
        }
        [Key]
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
