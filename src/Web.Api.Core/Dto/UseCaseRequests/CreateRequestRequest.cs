using System;
using System.Collections.Generic;
using System.Text;
using Web.Api.Core.Dto.UseCaseResponses;
using Web.Api.Core.Interfaces;

namespace Web.Api.Core.Dto.UseCaseRequests
{
    public class CreateRequestRequest : IUseCaseRequest<CreateRequestResponse>
    {
        //public Guid Id { get; set; }
        public Guid CreatedBy { get; set; }
        ////public DateTime? CreatedAt { get; set; }
        //public Guid? UpdatedBy { get; set; }
        //public DateTime? UpdatedAt { get; set; }
        //public Guid? DeletedBy { get; set; }
        //public DateTime? DeletedAt { get; set; }
        public string Title { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public Guid ServerId { get; set; }
        public string Description { get; set; }
        //public string Response { get; set; }
        //public Guid? ApprovedBy { get; set; }

        public CreateRequestRequest(Guid createdBy, string title, DateTime? startDate, DateTime? endDate, Guid serverId, string description)
        {
            //Id = id;
            CreatedBy = createdBy;
            //CreatedAt = createdAt;
            //UpdatedBy = updatedBy;
            //UpdatedAt = updatedAt;
            //DeletedBy = deletedBy;
            //DeletedAt = deletedAt;
            Title = title;
            StartDate = startDate;
            EndDate = endDate;
            ServerId = serverId;
            Description = description;
            //Response = response;
            //ApprovedBy = approvedBy;
            //RequestStatus = requestStatus;
        }
    }
}
