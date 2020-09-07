using System;
using Web.Api.Core.Dto.UseCaseResponses.User;
using Web.Api.Core.Interfaces;

namespace Web.Api.Core.Dto.UseCaseRequests.User
{
  public class ReadUserRequest : IUseCaseRequest<ReadUserResponse>
  {
    public ReadUserRequest(string uid)
    {
      UserId = uid;
      Page = -1;
      FilterString = "";
      SortedBy = "";
      SortOrder = "";
    }

    public ReadUserRequest(int page, int perPage, string filterString = "", string sortedBy = "Username", string sortOrder = "asc")
    {
      UserId = "";
      Page = page;
      PageSize = perPage;
      FilterString = filterString;
      SortedBy = sortedBy;
      SortOrder = sortOrder;
    }
    public string UserId { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
    public string SortedBy { get; set; }
    public string SortOrder { get; set; }
    public string FilterString { get; set; }
  }
}
