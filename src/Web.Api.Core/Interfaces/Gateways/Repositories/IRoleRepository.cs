using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Web.Api.Core.Interfaces.Gateways.Repositories
{
  public interface IRoleRepository
  {
    Task<bool> IsExisted(Guid Id);
  }
}
