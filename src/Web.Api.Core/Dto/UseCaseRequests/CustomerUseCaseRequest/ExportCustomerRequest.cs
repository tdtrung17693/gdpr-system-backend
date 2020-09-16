using System;
using System.Collections.Generic;
using System.Text;
using Web.Api.Core.Dto.UseCaseResponses;
using Web.Api.Core.Interfaces;

namespace Web.Api.Core.Dto.UseCaseRequests.CustomerUseCaseRequest
{
    public class ExportCustomerRequest : IUseCaseRequest<ExportCustomerResponse>
    {
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate {get; set;}
        public ICollection<Guid?> Guids { get; set; }
        public ExportCustomerRequest(DateTime? fromDate, DateTime? toDate, ICollection<Guid?> guids )
        {
            FromDate = fromDate;
            ToDate = toDate;
            Guids = guids;
        }
    }
}
