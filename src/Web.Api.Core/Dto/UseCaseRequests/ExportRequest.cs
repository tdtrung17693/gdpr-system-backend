using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Web.Api.Core.Dto.UseCaseResponses;
using Web.Api.Core.Interfaces;

namespace Web.Api.Core.Dto.UseCaseRequests
{
    public class ExportRequest : IUseCaseRequest<ExportResponse>
    {
        public DateTime fromDate { get; set; }
        public DateTime toDate { get; set; }
        public DataTable guids { get; set; }
        public ExportRequest(DateTime fromDateInput, DateTime toDateInput, DataTable guidsInput)
        {
            fromDate = fromDateInput;
            toDate = toDateInput;
            guids = guids;
        }
    }
}
