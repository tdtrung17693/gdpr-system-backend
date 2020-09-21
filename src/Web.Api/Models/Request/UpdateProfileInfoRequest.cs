using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Api.Models.Request
{
  public class UpdateProfileInfoRequest
  {
    public string FirstName { get; set; }
    public string LastName { get; set; }
  }
}
