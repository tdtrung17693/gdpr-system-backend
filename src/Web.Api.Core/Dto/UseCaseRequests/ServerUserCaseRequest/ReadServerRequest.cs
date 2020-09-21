using System;
using Web.Api.Core.Dto.UseCaseResponses;
using Web.Api.Core.Interfaces;

namespace Web.Api.Core.Dto.UseCaseRequests
{
    public class ReadServerRequest : IUseCaseRequest<ReadServerResponse>
    {
        public ReadServerRequest(Guid uid)
        {
            ServerId = uid;
            Page = -1;
            FilterString = "";
            SortedBy = "";
            SortOrder = "";
        }

        public ReadServerRequest(int page, int perPage, string filterString = "", string sortedBy = "Name", string sortOrder = "asc")
        {
            ServerId = Guid.Empty;
            Page = page;
            PageSize = perPage;
            FilterString = filterString;
            SortedBy = sortedBy;
            SortOrder = sortOrder;
        }
        public Guid ServerId { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public string SortedBy { get; set; }
        public string SortOrder { get; set; }
        public string FilterString { get; set; }
    }
}
