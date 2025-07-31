using ErrorOr;
using MediatR;
using WMS.Application.Common.Interface.Persistence;
using WMS.Application.Common.Interfaces.Persistence;
using WMS.Application.Resources.Common;
using WMS.Domain.Common.ErrorCatalog;

namespace WMS.Application.Resources.Commands.Update;

public class UpdateResourceCommandHandler :
    IRequestHandler<UpdateResourceCommand, ErrorOr<ResourceResult>>
{
    private readonly IResourceRepository _resourceRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateResourceCommandHandler(IResourceRepository resourceRepository, IUnitOfWork unitOfWork)
    {
        _resourceRepository = resourceRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ErrorOr<ResourceResult>> Handle(
        UpdateResourceCommand command,
        CancellationToken cancellationToken)
    {
        var resource = await _resourceRepository.GetByIdAsync(command.Id);
        if (resource is null)
            return Errors.Resource.NotFound;

        // Cannot update archived resource
        if (resource.IsArchived())
            return Errors.Resource.Archived;

        // Check for name conflicts with other resources
        var existingWithSameName = await _resourceRepository.GetByNameAsync(command.Name);
        if (existingWithSameName is not null && existingWithSameName.Id != resource.Id)
            return Errors.Resource.NameAlreadyExists;

        resource.ChangeName(command.Name);

        await _resourceRepository.UpdateAsync(resource);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new ResourceResult(resource.Id.Value, resource.Name, resource.IsActive);
    }
}