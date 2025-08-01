using ErrorOr;
using MediatR;
using WMS.Application.Common.Interface.Persistence;
using WMS.Application.UnitOfMeasurements.Common;
using WMS.Domain.Common.ErrorCatalog;
using WMS.Domain.UnitOfMeasurementAggregate.ValueObjects;

namespace WMS.Application.UnitOfMeasurements.Commands.Update;

public class UpdateUnitOfMeasurementCommandHandler :
    IRequestHandler<UpdateUnitOfMeasurementCommand, ErrorOr<UnitOfMeasurementResult>>
{
    private readonly IUnitOfMeasurementRepository _unitOfMeasurementRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateUnitOfMeasurementCommandHandler(IUnitOfMeasurementRepository unitOfMeasurementRepository, IUnitOfWork unitOfWork)
    {
        _unitOfMeasurementRepository = unitOfMeasurementRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ErrorOr<UnitOfMeasurementResult>> Handle(
    UpdateUnitOfMeasurementCommand command,
        CancellationToken cancellationToken)
    {
        var unitOfMeasurement = await _unitOfMeasurementRepository.GetByIdAsync(UnitOfMeasurementId.Create(command.Id));
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
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new UnitOfMeasurementResult(unitOfMeasurement.Id.Value, unitOfMeasurement.Name, unitOfMeasurement.IsActive);
    }
}