using System;
using System.Collections.Generic;
using System.Text;
using Web.Api.Core.Dto.UseCaseResponses.RequestUseCaseResponse;
using Web.Api.Core.Interfaces;

namespace Web.Api.Core.Dto.UseCaseRequests
{
    public class UpdateRequestRequest : IUseCaseRequest<UpdateRequestResponse>
    {
        public Guid Id { get; set; }
        //public Guid? CreatedBy { get; set; }
        //public DateTime? CreatedAt { get; set; }
        public Guid? UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        //public Guid? DeletedBy { get; set; }
        //public DateTime? DeletedAt { get; set; }
        public string Title { get; set; } 
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public Guid ServerId { get; set; }
        public string Description { get; set; }
        public string RequestStatus { get; set; }
        public string Response { get; set; }
        public Guid? ApprovedBy { get; set; }

        public UpdateRequestRequest(Guid id, Guid? updatedBy, DateTime? updatedAt, string title, DateTime? startDate, DateTime? endDate, Guid serverId, string description, string requestStatus, string response, Guid? approvedBy)
        {
            Id = id;
            //CreatedBy = createdBy;
            //CreatedAt = createdAt;
            UpdatedBy = updatedBy;
            UpdatedAt = updatedAt;
            //DeletedBy = deletedBy;
            //DeletedAt = deletedAt;
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
