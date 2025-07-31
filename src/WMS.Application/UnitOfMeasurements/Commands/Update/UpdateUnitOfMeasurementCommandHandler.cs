using ErrorOr;
using MediatR;
using WMS.Application.Common.Interface.Persistence;
using WMS.Application.UnitOfMeasurements.Common;
using WMS.Domain.Common.ErrorCatalog;

namespace WMS.Application.UnitOfMeasurements.Commands.Update;

public class UpdateUnitOfMeasurementCommandHandler :
    IRequestHandler<UpdateUnitOfMeasurementCommand, ErrorOr<UnitOfMeasurementResult>>
{
    private readonly IUnitOfMeasurementRepository _unitOfMeasurementRepository;

    public UpdateUnitOfMeasurementCommandHandler(IUnitOfMeasurementRepository unitOfMeasurementRepository)
    {
        _unitOfMeasurementRepository = unitOfMeasurementRepository;
    }

    public async Task<ErrorOr<UnitOfMeasurementResult>> Handle(
    UpdateUnitOfMeasurementCommand command,
        CancellationToken cancellationToken)
    {
        var unitOfMeasurement = await _unitOfMeasurementRepository.GetByIdAsync(command.Id);
        if (unitOfMeasurement is null)
            return Errors.UnitOfMeasurement.NotFound;

        // Cannot update archived unitOfMeasurement
        if (unitOfMeasurement.IsArchived())
            return Errors.UnitOfMeasurement.Archived;

        // Check for name conflicts with other unitOfMeasurement
        var existingWithSameName = await _unitOfMeasurementRepository.GetByNameAsync(command.Name);
        if (existingWithSameName is not null && existingWithSameName.Id != unitOfMeasurement.Id)
            return Errors.UnitOfMeasurement.NameAlreadyExists;

        unitOfMeasurement.ChangeName(command.Name);
        await _unitOfMeasurementRepository.UpdateAsync(unitOfMeasurement);

        return new UnitOfMeasurementResult(unitOfMeasurement.Id.Value, unitOfMeasurement.Name);
    }
}