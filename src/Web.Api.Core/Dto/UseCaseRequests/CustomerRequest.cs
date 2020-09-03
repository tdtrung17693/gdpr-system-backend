using System;
using System.Collections.Generic;
using System.Text;
using Web.Api.Core.Domain.Entities;
using Web.Api.Core.Dto.UseCaseResponses;
using Web.Api.Core.Interfaces;

namespace Web.Api.Core.Dto.UseCaseRequests
{
    public class CustomerRequest: IUseCaseRequest<CustomerResponse>
    {
        public Guid? Id { get; set; }
        public Guid? CreatedBy { get; set; }
        public Guid? UpdatedBy { get; set; }
        public string Name { get; set; }
        public DateTime? ContractBeginDate { get; set; }
        public DateTime? ContractEndDate { get; set; }
        public Guid? ContactPoint { get; set; }
        public string Description { get; set; }
        public bool? Status { get; set; }

        public virtual ICollection<CustomerServer> CustomerServer { get; set; }
        public CustomerRequest(string name,
          DateTime? contractBeginDate, DateTime? contractEndDate, Guid? contactPoint, string description, bool? status = true,
          Guid? id = null, Guid? createdBy = null, Guid? updatedBy = null)
        {
            Name = name;
            ContractBeginDate = contractBeginDate;
            ContractEndDate = contractEndDate;
            ContactPoint = contactPoint;
            Description = description;
            Status = status;
            Id = id;
        }
    }
}
