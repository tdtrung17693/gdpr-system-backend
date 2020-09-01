using System;
using System.Collections.Generic;
using System.Text;
using Web.Api.Core.Interfaces;

namespace Web.Api.Core.Dto.UseCaseRequests
{
    public class CustomerRequest
    {
        public Guid Id { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime? CreatedAt { get; set; }
        public Guid? UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string Name { get; set; }
        public DateTime? ContractBeginDate { get; set; }
        public DateTime? ContractEndDate { get; set; }
        public Guid? ContactPoint { get; set; }
        public string Description { get; set; }
        public bool? Status { get; set; }

        public CustomerRequest(Guid id, Guid? createdBy, DateTime? createdAt, Guid? updatedBy, DateTime? updatedAt, string name,
          DateTime? contractBeginDate, DateTime? contractEndDate, Guid? contactPoint, string description, bool? status)
        {
            Id = id;
            CreatedBy = createdBy;
            CreatedAt = createdAt;
            UpdatedBy = updatedBy;
            UpdatedAt = updatedAt;
            Name = name;
            ContractBeginDate = contractBeginDate;
            ContractEndDate = contractEndDate;
            ContactPoint = contactPoint;
            Description = description;
            Status = status;
        }
    }
}
