using System;
using System.Collections.Generic;
using System.Text;
using Web.Api.Core.Dto.UseCaseResponses;
using Web.Api.Core.Interfaces;

namespace Web.Api.Core.Dto.UseCaseRequests
{
    public class CreateRequestRequest : IUseCaseRequest<CreateRequestResponse>
    {
        public Guid CreatedBy { get; }
        public string Title { get; }
        public DateTime? StartDate { get; }
        public DateTime? EndDate { get; }
        public Guid ServerId { get; }
        public string Description { get; }

        public CreateRequestRequest(Guid createdBy, string title, DateTime? startDate, DateTime? endDate, Guid serverId,
            string description)
        {
            CreatedBy = createdBy;
            Title = title;
            StartDate = startDate;
            EndDate = endDate;
            ServerId = serverId;
            Description = description;
        }
    }
}