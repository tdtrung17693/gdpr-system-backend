using System;
using System.Collections.Generic;
using System.Text;
using Web.Api.Core.Dto.UseCaseResponses;
using Web.Api.Core.Dto.UseCaseResponses.ServerUseCaseResponse;
using Web.Api.Core.Interfaces;

namespace Web.Api.Core.Dto.UseCaseRequests.ServerUserCaseRequest
{
    public class UpdateServerRequest: IUseCaseRequest<UpdateServerResponse>
    {
        public Guid Id { get; set; }

        public Guid? CreatedBy { get; set; }

        public DateTime? CreatedAt { get; set; }

        public Guid? UpdatedBy { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public Guid? DeletedBy { get; set; }

        public DateTime? DeletedAt { get; set; }

        public bool? IsDeleted { get; set; }

        public string Name { get; set; }

        public string IpAddress { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public bool? Status { get; set; }
        public UpdateServerRequest(Guid id, DateTime? createdAt, Guid? createdBy, DateTime? deletedAt, Guid? deletedBy
           , DateTime? endDate, string ipAddress, bool? isDeleted, string name, DateTime? startDate, bool? status, DateTime? updatedAt, Guid? updatedBy)
        {
            Id = id;

            CreatedBy = createdBy;

            CreatedAt = createdAt;

            UpdatedBy = updatedBy;

            UpdatedAt = updatedAt;

            DeletedBy = deletedBy;

            DeletedAt = deletedAt;

            IsDeleted = isDeleted;

            Name = name;

            IpAddress = ipAddress;

            StartDate = startDate;

            EndDate = endDate;

            Status = status;
        }
    }
   
}
