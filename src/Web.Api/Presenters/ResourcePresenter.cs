
using System;
using System.Net;
using Web.Api.Core.Interfaces;
using Web.Api.Serialization;

namespace Web.Api.Presenters
{
  public sealed class ResourcePresenter<T> : IOutputPort<T> where T:UseCaseResponseMessage
  {
    public JsonContentResult ContentResult { get; }
    public Func<T, string> HandleResource { get; set; }
    
    public ResourcePresenter()
    {
      ContentResult = new JsonContentResult();
    }
    public void Handle(T response)
    {
      ContentResult.StatusCode = (int)(response.Success ? HttpStatusCode.OK : HttpStatusCode.Unauthorized);
      ContentResult.Content = HandleResource(response);
    }

  }
}
