using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Web.Api.Core.Dto.UseCaseResponses;
using Web.Api.Core.Interfaces;
using Web.Api.Serialization;

namespace Web.Api.Presenters
{
    public class CustomerPresenter: IOutputPort<CustomerResponse>
    {
        public JsonContentResult ContentResult { get; }

        public CustomerPresenter()
        {
            ContentResult = new JsonContentResult();
        }

        public void Handle(CustomerResponse response)
        {
            ContentResult.StatusCode = (int)(response.Success ? HttpStatusCode.OK : HttpStatusCode.Unauthorized);
            ContentResult.Content = response.Success ? JsonSerializer.SerializeObject(response) : JsonSerializer.SerializeObject(response.Errors);
        }
    }
}
