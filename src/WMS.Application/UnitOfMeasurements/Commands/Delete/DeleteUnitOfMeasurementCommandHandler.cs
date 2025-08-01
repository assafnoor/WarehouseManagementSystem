using ErrorOr;
using MediatR;
using WMS.Application.Common.Interface.Persistence;
using WMS.Application.UnitOfMeasurements.Common;
using WMS.Domain.Common.ErrorCatalog;
using WMS.Domain.UnitOfMeasurementAggregate.ValueObjects;

namespace WMS.Application.UnitOfMeasurements.Commands.Delete;

public class DeleteUnitOfMeasurementCommandHandler :
    IRequestHandler<DeleteUnitOfMeasurementCommand, ErrorOr<UnitOfMeasurementResult>>
{
    private readonly IUnitOfMeasurementRepository _unitOfMeasurementRepository;

    public DeleteUnitOfMeasurementCommandHandler(IUnitOfMeasurementRepository unitOfMeasurementRepository)
    {
        _unitOfMeasurementRepository = unitOfMeasurementRepository;
    }

    public async Task<ErrorOr<UnitOfMeasurementResult>> Handle(
    DeleteUnitOfMeasurementCommand command,
        CancellationToken cancellationToken)
    {
        var unitOfMeasurement = await _unitOfMeasurementRepository.GetByIdAsync(UnitOfMeasurementId.Create(command.Id));
        if (unitOfMeasurement is null)
            return Errors.UnitOfMeasurement.NotFound;

        // Check if unitOfMeasurement is used in documents
        var isUsedInDocuments = await _unitOfMeasurementRepository.IsUsedInDocumentsAsync(UnitOfMeasurementId.Create(command.Id));

        // Try to archive the unitOfMeasurement (this includes business logic validation)
        var archiveResult = unitOfMeasurement.CanBeArchived(isUsedInDocuments);
        if (archiveResult.IsError)
            return archiveResult.Errors;

        await _unitOfMeasurementRepository.UpdateAsync(unitOfMeasurement);

        return new UnitOfMeasurementResult(unitOfMeasurement.Id.Value, unitOfMeasurement.Name, unitOfMeasurement.IsActive);
    }
}