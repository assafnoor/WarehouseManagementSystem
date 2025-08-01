using ErrorOr;
using MediatR;
using WMS.Application.Common.Interface.Persistence;
using WMS.Application.Resources.Common;
using WMS.Domain.Common.ErrorCatalog;
using WMS.Domain.ResourceAggregate.ValueObjects;

namespace WMS.Application.Resources.Queries.GetById;

public class GetByIdResourceQueryHandler :
    IRequestHandler<GetByIdResourceQuery, ErrorOr<ResourceResult>>
{
    private readonly IResourceRepository _resourceRepository;

    public GetByIdResourceQueryHandler(IResourceRepository resourceRepository)
    {
        _resourceRepository = resourceRepository;
    }

    public async Task<ErrorOr<ResourceResult>> Handle(
        GetByIdResourceQuery query,
        CancellationToken cancellationToken)
    {
        var resource = await _resourceRepository.GetByIdAsync(ResourceId.Create(query.Id));

        if (resource is null)
            return Errors.Resource.NotFound;

        return new ResourceResult(resource.Id.Value, resource.Name, resource.IsActive);
    }
}