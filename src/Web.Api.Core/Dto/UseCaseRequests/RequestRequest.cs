﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Web.Api.Core.Dto.UseCaseRequests
{
    class RequestRequest
    {
        public Guid Id { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public Guid? UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public Guid? DeletedBy { get; set; }
        public DateTime? DeletedAt { get; set; }
        public string RequestStatus { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public Guid? ServerId { get; set; }
        public string Response { get; set; }
        public Guid? ApprovedBy { get; set; }

        public RequestRequest(Guid id, Guid createdBy, DateTime createdAt, Guid? updatedBy, DateTime? updatedAt, Guid? deletedBy, DateTime? deletedAt, string title, string description, DateTime startDate, DateTime endDate, Guid? serverId, string requestStatus, string response, Guid? approvedBy, bool? status)
        {
            Id = id;
            CreatedBy = createdBy;
            CreatedAt = createdAt;
            UpdatedBy = updatedBy;
            UpdatedAt = updatedAt;
            DeletedBy = deletedBy;
            DeletedAt = deletedAt;
            Title = title;
            Description = description;
            StartDate = startDate;
            EndDate = endDate;
            ServerId = serverId;
            Response = response;
            ApprovedBy = approvedBy;
            RequestStatus = requestStatus;
        }
    }
}
