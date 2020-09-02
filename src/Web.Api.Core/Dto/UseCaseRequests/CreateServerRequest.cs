using System;
using System.Collections.Generic;
using System.Text;
using Web.Api.Core.Domain.Entities;
using Web.Api.Core.Dto.UseCaseResponses;
using Web.Api.Core.Interfaces;

namespace Web.Api.Core.Dto.UseCaseRequests
{
    public class CreateServerRequest : IUseCaseRequest<CreateNewServerResponse>
    {


        public Guid Id { get; set; }

        public Guid CreatedBy { get; set; }

        public DateTime CreatedAt { get; set; }

        public Guid? UpdatedBy { get; set; }

        public Nullable<DateTime> UpdatedAt { get; set; }

        public Guid? DeletedBy { get; set; }

        public Nullable<DateTime> DeletedAt { get; set; }

        public bool IsDeleted { get; set; }

        public string Name { get; set; }

        public string IpAddress { get; set; }

        public Nullable<DateTime> StartDate { get; set; }

        public Nullable<DateTime> EndDate { get; set; }

        public bool? Status { get; set; }

        public virtual User CreatedByNavigation { get; set; }

        public virtual User DeletedByNavigation { get; set; }

        public virtual User UpdatedByNavigation { get; set; }

        public CreateServerRequest(Guid Id, DateTime CreatedAt, Guid CreatedBy, Nullable<DateTime> UpdatedAt, Guid? UpdatedBy,
             Nullable<DateTime> DeletedAt, Guid? DeletedBy, bool isDeleted, bool? status, string Name, string IpAddress, Nullable<DateTime> StartDate, 
            Nullable<DateTime> EndDate)
        {
            this.Id = Id;

            this.CreatedBy = CreatedBy;

            this.CreatedAt = CreatedAt;

            this.UpdatedBy = UpdatedBy ;

            this.UpdatedAt = UpdatedAt;

            this.DeletedBy = DeletedBy ;

            this.DeletedAt = DeletedAt;

            this.IsDeleted = isDeleted;

            this.Name = Name;

            this.IpAddress = IpAddress;

            this.StartDate = StartDate;

            this.EndDate = EndDate;

            this.Status = status;
        }
    }
}
