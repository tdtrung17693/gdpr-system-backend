using System.Collections.Generic;

namespace Web.Api.Core.Dto.GatewayResponses.Repositories
{
  public sealed class UpdateUserResponse : BaseGatewayResponse
  {
    public IDictionary<string, string> UpdatedFields { get; }

    public UpdateUserResponse(bool success, IEnumerable<Error> errors = null) : base(success, errors)
    {
    }
    public UpdateUserResponse(IDictionary<string, string> updatedFields, bool success = true, IEnumerable<Error> errors = null) : base(success, errors)
    {
      UpdatedFields = updatedFields;
    }
  }
}
