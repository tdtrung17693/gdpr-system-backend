using System.Threading.Tasks;
using Web.Api.Core.Dto.UseCaseRequests;
using Web.Api.Core.Dto.UseCaseResponses;

namespace Web.Api.Core.Interfaces.UseCases.RequestInterface
{
    public interface IBulkRequestUseCase : IUseCaseRequestHandler<BulkRequestRequest, BulkRequestResponse>
    {
    }
}
