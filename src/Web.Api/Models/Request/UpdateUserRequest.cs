using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Api.Models.Request
{
  public class UpdateUserRequest
  {
    public UpdateUserRequest(Guid roleId, bool status)
    {
      RoleId = roleId;
      Status = status;
    }
    public Guid RoleId { get; set; }
    public bool Status { get; set; }
  }
}
