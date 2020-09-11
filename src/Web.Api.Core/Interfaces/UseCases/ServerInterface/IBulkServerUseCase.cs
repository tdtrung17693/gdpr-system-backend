using System.Threading.Tasks;
using Web.Api.Core.Dto.UseCaseRequests.ServerUserCaseRequest;
using Web.Api.Core.Dto.UseCaseResponses.ServerUseCaseResponse;

namespace Web.Api.Core.Interfaces.UseCases.ServerInterface
{
    public interface IBulkServerUseCase : IUseCaseRequestHandler<BulkServerRequest, BulkServerResponse>
    {
    }
}
