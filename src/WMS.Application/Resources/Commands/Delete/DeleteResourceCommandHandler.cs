using ErrorOr;
using MediatR;
using MyApp.Application.Common.Interfaces.Persistence;
using WMS.Application.Resources.Common;
using WMS.Domain.Common.ErrorCatalog;

namespace WMS.Application.Resources.Commands.Delete;

public class DeleteResourceCommandHandler :
    IRequestHandler<DeleteResourceCommand, ErrorOr<ResourceResult>>
{
    private readonly IResourceRepository _resourceRepository;

    public DeleteResourceCommandHandler(IResourceRepository resourceRepository)
    {
        _resourceRepository = resourceRepository;
    }

    public async Task<ErrorOr<ResourceResult>> Handle(
        DeleteResourceCommand command,
        CancellationToken cancellationToken)
    {
        var resource = await _resourceRepository.GetByIdAsync(command.Id);
        if (resource is null)
            return Errors.Resource.NotFound;

        // Check if resource is used in documents
        var isUsedInDocuments = await _resourceRepository.IsResourceUsedInDocumentsAsync(command.Id);

        // Try to archive the resource (this includes business logic validation)
        var archiveResult = resource.CanBeArchived(isUsedInDocuments);
        if (archiveResult.IsError)
            return archiveResult.Errors;

        await _resourceRepository.UpdateAsync(resource);

        return new ResourceResult(resource.Id.Value, resource.Name);
    }
}