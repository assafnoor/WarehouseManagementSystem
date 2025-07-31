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
    private readonly IUnitOfWork _unitOfWork;

    public ArchiveUnitOfMeasurementCommandHandler(IUnitOfMeasurementRepository unitOfMeasurementRepository, IUnitOfWork unitOfWork)
    {
        _unitOfMeasurementRepository = unitOfMeasurementRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ErrorOr<UnitOfMeasurementResult>> Handle(
        ArchiveUnitOfMeasurementCommand command,
        CancellationToken cancellationToken)
    {
        var unitOfMeasurement = await _unitOfMeasurementRepository.GetByIdAsync(command.Id);
        if (unitOfMeasurement is null)
            return Errors.UnitOfMeasurement.NotFound;

        //var canBeArchived = client.CanBeArchived(false);// return
        //if (canBeArchived.IsError)
        //    return canBeArchived.Errors;

        var activateResult = unitOfMeasurement.Archive();
        if (activateResult.IsError)
            return activateResult.Errors;

        await _unitOfMeasurementRepository.UpdateAsync(unitOfMeasurement);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new UnitOfMeasurementResult(unitOfMeasurement.Id.Value, unitOfMeasurement.Name, unitOfMeasurement.IsActive);
    }
}