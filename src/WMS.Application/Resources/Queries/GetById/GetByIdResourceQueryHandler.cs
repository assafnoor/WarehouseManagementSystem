using ErrorOr;
using MediatR;
using WMS.Application.Common.Interfaces.Persistence;
using WMS.Application.Resources.Common;
using WMS.Domain.Common.ErrorCatalog;

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
        var resource = await _resourceRepository.GetByIdAsync(query.Id);

        if (resource is null)
            return Errors.Resource.NotFound;

        return new ResourceResult(resource.Id.Value, resource.Name, resource.IsActive);
    }
}