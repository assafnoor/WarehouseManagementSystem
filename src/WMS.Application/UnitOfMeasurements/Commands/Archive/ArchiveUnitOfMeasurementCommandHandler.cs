using ErrorOr;
using MediatR;
using WMS.Application.Common.Interface.Persistence;
using WMS.Application.UnitOfMeasurements.Common;
using WMS.Domain.Common.ErrorCatalog;

namespace WMS.Application.UnitOfMeasurements.Commands.Archive;

public class ArchiveUnitOfMeasurementCommandHandler :
    IRequestHandler<ArchiveUnitOfMeasurementCommand, ErrorOr<UnitOfMeasurementResult>>
{
    private readonly IUnitOfMeasurementRepository _unitOfMeasurementRepository;

    public ArchiveUnitOfMeasurementCommandHandler(IUnitOfMeasurementRepository unitOfMeasurementRepository)
    {
        _unitOfMeasurementRepository = unitOfMeasurementRepository;
    }

    public async Task<ErrorOr<UnitOfMeasurementResult>> Handle(
        ArchiveUnitOfMeasurementCommand command,
        CancellationToken cancellationToken)
    {
        var unitOfMeasurement = await _unitOfMeasurementRepository.GetByIdAsync(command.Id);
        if (unitOfMeasurement is null)
            return Errors.UnitOfMeasurement.NotFound;

        var activateResult = unitOfMeasurement.Archive();
        if (activateResult.IsError)
            return activateResult.Errors;

        await _unitOfMeasurementRepository.UpdateAsync(unitOfMeasurement);

        return new UnitOfMeasurementResult(unitOfMeasurement.Id.Value, unitOfMeasurement.Name);
    }
}