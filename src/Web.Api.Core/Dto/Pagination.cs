using System;
using System.Collections.Generic;
using System.Text;

namespace Web.Api.Core.Dto
{
  public class Pagination<T>
  {
    public IEnumerable<T> Items { get; set; }
    public int Page { get; set; }
    public int TotalPages { get; set; }
    public int TotalItems { get; set; }
  }
}
