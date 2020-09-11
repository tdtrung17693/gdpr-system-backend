using System;
using System.Collections.Generic;
using System.Text;

namespace Web.Api.Core.Dto.UseCaseResponses.User
{
  public class UserDTO
  {
    public string Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Username { get; set; }
    public string RoleId { get; set; }
    public string RoleName { get; set; }
    public bool Status { get; set; }
  }
}
