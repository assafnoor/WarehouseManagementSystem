using ErrorOr;
using MediatR;
using WMS.Application.Common.Interface.Persistence;
using WMS.Application.UnitOfMeasurements.Common;
using WMS.Domain.Common.ErrorCatalog;
using WMS.Domain.UnitOfMeasurementAggregate;

namespace WMS.Application.UnitOfMeasurements.Commands.Create;

public class CreateUnitOfMeasurementCommandHandler :
    IRequestHandler<CreateUnitOfMeasurementCommand, ErrorOr<UnitOfMeasurementResult>>
{
    private readonly IUnitOfMeasurementRepository _unitOfMeasurementRepository;

    public CreateUnitOfMeasurementCommandHandler(IUnitOfMeasurementRepository unitOfMeasurementRepository)
    {
        _unitOfMeasurementRepository = unitOfMeasurementRepository;
    }

    public async Task<ErrorOr<UnitOfMeasurementResult>> Handle(
        CreateUnitOfMeasurementCommand command,
        CancellationToken cancellationToken)
    {
        // Check if any UnitOfMeasurement with this name exists (active or archived)
        var existingResource = await _unitOfMeasurementRepository.GetByNameAsync(command.Name);
        if (existingResource is not null)
        {
            return Errors.UnitOfMeasurement.NameAlreadyExists;
        }

        var resource = UnitOfMeasurement.Create(command.Name);
        await _unitOfMeasurementRepository.AddAsync(resource);

        return new UnitOfMeasurementResult(resource.Id.Value, resource.Name);
    }
}