using ErrorOr;
using MediatR;
using WMS.Application.Common.Interface.Persistence;
using WMS.Application.UnitOfMeasurements.Common;
using WMS.Domain.Common.ErrorCatalog;

namespace WMS.Application.UnitOfMeasurements.Commands.Activate;

public class ActivateUnitOfMeasurementCommandHandler :
    IRequestHandler<ActivateUnitOfMeasurementCommand, ErrorOr<UnitOfMeasurementResult>>
{
    private readonly IUnitOfMeasurementRepository _unitOfMeasurementRepository;

    public ActivateUnitOfMeasurementCommandHandler(IUnitOfMeasurementRepository unitOfMeasurementRepository)
    {
        _unitOfMeasurementRepository = unitOfMeasurementRepository;
    }

    public async Task<ErrorOr<UnitOfMeasurementResult>> Handle(
        ActivateUnitOfMeasurementCommand command,
        CancellationToken cancellationToken)
    {
        var unitOfMeasurement = await _unitOfMeasurementRepository.GetByIdAsync(command.Id);
        if (unitOfMeasurement is null)
            return Errors.UnitOfMeasurement.NotFound;

        var activateResult = unitOfMeasurement.Activate();
        if (activateResult.IsError)
            return activateResult.Errors;

        await _unitOfMeasurementRepository.UpdateAsync(unitOfMeasurement);

        return new UnitOfMeasurementResult(unitOfMeasurement.Id.Value, unitOfMeasurement.Name);
    }
}