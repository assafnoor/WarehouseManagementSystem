using ErrorOr;
using MediatR;
using WMS.Application.Common.Interface.Persistence;
using WMS.Application.Resources.Common;
using WMS.Domain.ResourceAggregate;

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
        var resources = await _resourceRepository.GetAllAsync(query.Status, query.Page, query.PageSize)
            ?? Array.Empty<Resource>();
        var results = resources.Select(r => new ResourceResult(r.Id.Value, r.Name, r.IsActive));
        return results.ToList();
    }
}