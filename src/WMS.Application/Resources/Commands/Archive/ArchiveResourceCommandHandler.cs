using ErrorOr;
using MediatR;
using MyApp.Application.Common.Interfaces.Persistance;
using WMS.Application.Resources.Common;
using WMS.Domain.Common.ErrorCatalog;

namespace WMS.Application.Resources.Commands.Archive;

public class ArchiveResourceCommandHandler :
    IRequestHandler<ArchiveResourceCommand, ErrorOr<ResourceResult>>
{
    private readonly IResourceRepository _resourceRepository;

    public ArchiveResourceCommandHandler(IResourceRepository esourceRepository)
    {
        _resourceRepository = esourceRepository;
    }

    public async Task<ErrorOr<ResourceResult>> Handle(
        ArchiveResourceCommand command,
        CancellationToken cancellationToken)
    {
        var resource = await _resourceRepository.GetByIdAsync(command.Id);
        if (resource is null)
            return Errors.Resource.NotFound;

        var activateResult = resource.Archive();
        if (activateResult.IsError)
            return activateResult.Errors;

        await _resourceRepository.UpdateAsync(resource);

        return new ResourceResult(resource.Id.Value, resource.Name);
    }
}