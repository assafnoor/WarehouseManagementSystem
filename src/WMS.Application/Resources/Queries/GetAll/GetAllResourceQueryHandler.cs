using ErrorOr;
using MediatR;
using MyApp.Application.Common.Interfaces.Persistance;
using WMS.Application.Resources.Common;

namespace WMS.Application.Resources.Queries.GetAll;

public class GetAllResourceQueryHandler :
    IRequestHandler<GetAllResourceQuery, ErrorOr<IEnumerable<ResourceResult>>>
{
    private readonly IResourceRepository _resourceRepository;

    public GetAllResourceQueryHandler(IResourceRepository resourceRepository)
    {
        _resourceRepository = resourceRepository;
    }

    public async Task<ErrorOr<IEnumerable<ResourceResult>>> Handle(
        GetAllResourceQuery query,
        CancellationToken cancellationToken)
    {
        var resources = await _resourceRepository.GetActiveResourcesAsync();
        var results = resources.Select(r => new ResourceResult(r.Id.Value, r.Name));
        return results.ToList();
    }
}