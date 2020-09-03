
using System;
using System.Collections.Generic;
using Web.Api.Core.Domain.Entities;

namespace Web.Api.Models.Request
{
    public class CustomerRequest
    {
        public Guid Id { get; set; }
        public Guid? CreatedBy { get; set; }
        public Guid? UpdatedBy { get; set; }
        public string Name { get; set; }
        public DateTime? ContractBeginDate { get; set; }
        public DateTime? ContractEndDate { get; set; }
        public Guid? ContactPoint { get; set; }
        public string Description { get; set; }
        public bool? Status { get; set; }
    }
}