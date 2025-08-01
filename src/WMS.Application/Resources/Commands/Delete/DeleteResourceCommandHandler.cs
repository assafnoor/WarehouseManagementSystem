﻿using ErrorOr;
using MediatR;
using WMS.Application.Common.Interface.Persistence;
using WMS.Application.Resources.Common;
using WMS.Domain.Common.ErrorCatalog;
using WMS.Domain.ResourceAggregate.ValueObjects;

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
        var resource = await _resourceRepository.GetByIdAsync(ResourceId.Create(command.Id));
        if (resource is null)
            return Errors.Resource.NotFound;

        // Check if resource is used in documents
        var isUsedInDocuments = await _resourceRepository.IsUsedInDocumentsAsync(ResourceId.Create(command.Id));

        // Try to archive the resource (this includes business logic validation)
        var archiveResult = resource.CanBeArchived(isUsedInDocuments);
        if (archiveResult.IsError)
            return archiveResult.Errors;

        await _resourceRepository.UpdateAsync(resource);

        return new ResourceResult(resource.Id.Value, resource.Name, resource.IsActive);
    }
}