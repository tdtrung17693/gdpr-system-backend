using System.Net;
using Web.Api.Core.Dto.UseCaseResponses.User;
using Web.Api.Core.Interfaces;
using Web.Api.Serialization;

namespace Web.Api.Presenters
{
  public sealed class CreateEntityPresenter : IOutputPort<CreateUserResponse>
  {
    public JsonContentResult ContentResult { get; }

    public CreateEntityPresenter()
    {
      ContentResult = new JsonContentResult();
    }

    public void Handle(CreateUserResponse response)
    {
      ContentResult.StatusCode = (int)(response.Success ? HttpStatusCode.OK : HttpStatusCode.BadRequest);
      ContentResult.Content = JsonSerializer.SerializeObject(response);
    }
  }
}
