using ErrorOr;
using MediatR;
using MyApp.Application.Common.Interfaces.Persistance;
using WMS.Application.Resources.Common;
using WMS.Domain.Common.ErrorCatalog;

namespace WMS.Application.Resources.Commands.Activate;

public class ActivateResourceCommandHandler :
    IRequestHandler<ActivateResourceCommand, ErrorOr<ResourceResult>>
{
    private readonly IResourceRepository _resourceRepository;

    public ActivateResourceCommandHandler(IResourceRepository resourceRepository)
    {
        _resourceRepository = resourceRepository;
    }

    public async Task<ErrorOr<ResourceResult>> Handle(
        ActivateResourceCommand command,
        CancellationToken cancellationToken)
    {
        var resource = await _resourceRepository.GetByIdAsync(command.Id);
        if (resource is null)
            return Errors.Resource.NotFound;

        var activateResult = resource.Activate();
        if (activateResult.IsError)
            return activateResult.Errors;

        await _resourceRepository.UpdateAsync(resource);

        return new ResourceResult(resource.Id.Value, resource.Name);
    }
}