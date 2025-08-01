using ErrorOr;
using MediatR;
using WMS.Application.Common.Interface.Persistence;
using WMS.Application.UnitOfMeasurements.Common;
using WMS.Domain.Common.ErrorCatalog;
using WMS.Domain.UnitOfMeasurementAggregate.ValueObjects;

namespace WMS.Application.UnitOfMeasurements.Commands.Activate;

public class ActivateUnitOfMeasurementCommandHandler :
    IRequestHandler<ActivateUnitOfMeasurementCommand, ErrorOr<UnitOfMeasurementResult>>
{
    private readonly IUnitOfMeasurementRepository _unitOfMeasurementRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ActivateUnitOfMeasurementCommandHandler(IUnitOfMeasurementRepository unitOfMeasurementRepository, IUnitOfWork unitOfWork)
    {
        _unitOfMeasurementRepository = unitOfMeasurementRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ErrorOr<UnitOfMeasurementResult>> Handle(
        ActivateUnitOfMeasurementCommand command,
        CancellationToken cancellationToken)
    {
        var unitOfMeasurement = await _unitOfMeasurementRepository.GetByIdAsync(UnitOfMeasurementId.Create(command.Id));
        if (unitOfMeasurement is null)
            return Errors.UnitOfMeasurement.NotFound;

        var activateResult = unitOfMeasurement.Activate();
        if (activateResult.IsError)
            return activateResult.Errors;

        await _unitOfMeasurementRepository.UpdateAsync(unitOfMeasurement);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return new UnitOfMeasurementResult(unitOfMeasurement.Id.Value, unitOfMeasurement.Name, unitOfMeasurement.IsActive);
    }
}