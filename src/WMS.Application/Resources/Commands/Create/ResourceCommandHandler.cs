using ErrorOr;
using MediatR;
using MyApp.Application.Common.Interfaces.Persistance;
using WMS.Application.Resources.Common;
using WMS.Domain.Common.ErrorCatalog;
using WMS.Domain.ResourceAggregate;

namespace WMS.Application.Resources.Commands.Create;

public class ResourceCommandHandler :
    IRequestHandler<ResourceCommand, ErrorOr<ResourceResult>>
{
    private readonly IResourceRepository _resourceRepository;

    public ResourceCommandHandler(
                IResourceRepository resourceRepository)
    {
        _resourceRepository = resourceRepository;
    }

    public async Task<ErrorOr<ResourceResult>> Handle(
        ResourceCommand command,
        CancellationToken cancellationToken)
    {
        var existingResource = await _resourceRepository.GetByNameAsync(command.Name);
        if (existingResource is not null)
        {
            return Errors.Resource.NameAlreadyExists;
        }
        var resource = Resource.Create(command.Name);
        await _resourceRepository.AddAsync(resource);

        return new ResourceResult(
            resource.Id.Value,
            resource.Name
            );
    }
}