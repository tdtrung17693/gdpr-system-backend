using System.Threading.Tasks;
using Web.Api.Core.Dto.UseCaseRequests;
using Web.Api.Core.Dto.UseCaseRequests.ServerUserCaseRequest;
using Web.Api.Core.Dto.UseCaseResponses;
using Web.Api.Core.Dto.UseCaseResponses.ServerUseCaseResponse;

namespace Web.Api.Core.Interfaces.UseCases.ServerInterface
{
    public interface IUpdateServerUseCase : IUseCaseRequestHandler<UpdateServerRequest, UpdateServerResponse>
    {
    }
}
