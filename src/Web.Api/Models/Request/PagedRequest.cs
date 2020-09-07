using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Api.Models.Request
{
  public class PagedRequest
  {
    public PagedRequest()
    {
      Page = 1;
      PageSize = 10;
      FilterBy = "";
      SortedBy = "CreatedAt";
      SortOrder = "";
    }
    public int Page { get; set; }
    public int PageSize { get; set; }
    public string FilterBy { get; set; }
    public string SortedBy { get; set; }
    public string SortOrder { get; set; }
  }
}
