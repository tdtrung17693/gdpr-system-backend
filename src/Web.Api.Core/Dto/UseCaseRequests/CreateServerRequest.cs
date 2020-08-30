using System;
using System.Collections.Generic;
using System.Text;
using Web.Api.Core.Dto.UseCaseResponses;
using Web.Api.Core.Interfaces;

namespace Web.Api.Core.Dto.UseCaseRequests
{
    public class CreateServerRequest : IUseCaseRequest<CreateNewServerResponse>
    {
        public Guid Id { get; }

        public Guid CreatedBy { get; }

        public DateTime CreatedAt { get; }

        public Guid? UpdatedBy { get; }

        public Nullable<DateTime> UpdatedAt { get; }

        public Guid? DeletedBy { get; }

        public Nullable<DateTime> DeletedAt { get; }

        public bool isDeleted { get; }

        public string Name { get; }

        public string IpAddress { get; }

        public Nullable<DateTime> StartDate { get; }

        public Nullable<DateTime> EndDate { get; }

        public CreateServerRequest(Guid Id, Guid CreatedBy, DateTime CreatedAt, Guid? UpdatedBy, Nullable<DateTime> UpdatedAt, Guid? DeletedBy, Nullable<DateTime> DeletedAt, bool isDeleted, string Name, string IpAddress, Nullable<DateTime> StartDate, Nullable<DateTime> EndDate)
        {
            this.Id = Id;

            this.CreatedBy = CreatedBy;

            this.CreatedAt = CreatedAt;

            this.UpdatedBy = UpdatedBy == null ? Guid.Empty : UpdatedBy;

            this.UpdatedAt = UpdatedAt;

            this.DeletedBy = DeletedBy == null ? Guid.Empty : DeletedBy;

            this.DeletedAt = DeletedAt;

            this.isDeleted = isDeleted;

            this.Name = Name;

            this.IpAddress = IpAddress;

            this.StartDate = StartDate;

            this.EndDate = EndDate;
        }
    }
}
