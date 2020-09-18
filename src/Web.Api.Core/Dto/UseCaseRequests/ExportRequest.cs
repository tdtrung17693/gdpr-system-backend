using System;
using System.Collections.Generic;
using System.Text;
using Web.Api.Core.Dto.UseCaseResponses;
using Web.Api.Core.Interfaces;

namespace Web.Api.Core.Dto.UseCaseRequests
{
    public class ExportRequest : IUseCaseRequest<ExportResponse>
    {
        public DateTime fromDate { get; set; }
        public DateTime toDate { get; set; }
        public ICollection<Guid?> guids { get; set; }
        public ExportRequest(DateTime fromDateInput, DateTime toDateInput, ICollection<Guid?> guidsInput)
        {
            fromDate = fromDateInput;
            toDate = toDateInput;
            guids = guids;
        }
    }
}
