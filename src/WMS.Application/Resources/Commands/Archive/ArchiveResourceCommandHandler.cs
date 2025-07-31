using ErrorOr;
using MediatR;
using WMS.Application.Common.Interface.Persistence;
using WMS.Application.Common.Interfaces.Persistence;
using WMS.Application.Resources.Common;
using WMS.Domain.Common.ErrorCatalog;

namespace WMS.Application.Resources.Commands.Archive;

public class ArchiveResourceCommandHandler :
    IRequestHandler<ArchiveResourceCommand, ErrorOr<ResourceResult>>
{
    private readonly IResourceRepository _resourceRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ArchiveResourceCommandHandler(IResourceRepository resourceRepository, IUnitOfWork unitOfWork)
    {
        _resourceRepository = resourceRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ErrorOr<ResourceResult>> Handle(
        ArchiveResourceCommand command,
        CancellationToken cancellationToken)
    {
        var resource = await _resourceRepository.GetByIdAsync(command.Id);
        if (resource is null)
            return Errors.Resource.NotFound;

        //var canBeArchived = client.CanBeArchived(false);// return
        //if (canBeArchived.IsError)
        //    return canBeArchived.Errors;

        var activateResult = resource.Archive();
        if (activateResult.IsError)
            return activateResult.Errors;

        await _resourceRepository.UpdateAsync(resource);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new ResourceResult(resource.Id.Value, resource.Name, resource.IsActive);
    }
}