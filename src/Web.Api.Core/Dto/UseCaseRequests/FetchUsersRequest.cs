using System;
using System.Collections.Generic;
using System.Text;
using Web.Api.Core.Domain.Entities;

namespace Web.Api.Core.Dto.UseCaseRequests
{
    class FetchUsersRequest
    {
        public string FilterString { get; }
        public int Page { get; }
        public string SortedBy { get; }
        public string SortOrder { get; }

        public FetchUsersRequest(int page, string sortedBy, string sortOrder, string filterString)
        {
            Page = page;
            SortedBy = sortedBy;
            SortOrder = sortOrder;
            FilterString = filterString;
        }
    }
}
