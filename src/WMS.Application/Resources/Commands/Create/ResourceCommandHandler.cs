using ErrorOr;
using MediatR;
using WMS.Application.Common.Interface.Persistence;
using WMS.Application.Resources.Common;
using WMS.Domain.Common.ErrorCatalog;
using WMS.Domain.ResourceAggregate;

namespace WMS.Application.Resources.Commands.Create;

public class ResourceCommandHandler :
    IRequestHandler<ResourceCommand, ErrorOr<ResourceResult>>
{
    private readonly IResourceRepository _resourceRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ResourceCommandHandler(
                IResourceRepository resourceRepository, IUnitOfWork unitOfWork)
    {
        _resourceRepository = resourceRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ErrorOr<ResourceResult>> Handle(
        ResourceCommand command,
        CancellationToken cancellationToken)
    {
        // Check if any resource with this name exists (active or archived)
        var existingResource = await _resourceRepository.GetByNameAsync(command.Name);
        if (existingResource is not null)
        {
            return Errors.Resource.NameAlreadyExists;
        }

        var resource = Resource.Create(command.Name);

        await _resourceRepository.AddAsync(resource);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new ResourceResult(resource.Id.Value, resource.Name, resource.IsActive);
    }
}