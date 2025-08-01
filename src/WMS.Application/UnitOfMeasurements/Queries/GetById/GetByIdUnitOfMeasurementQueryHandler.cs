using ErrorOr;
using MediatR;
using WMS.Application.Common.Interface.Persistence;
using WMS.Application.UnitOfMeasurements.Common;
using WMS.Domain.Common.ErrorCatalog;
using WMS.Domain.UnitOfMeasurementAggregate.ValueObjects;

namespace WMS.Application.UnitOfMeasurements.Queries.GetById;

public class GetByIdUnitOfMeasurementQueryHandler :
    IRequestHandler<GetByIdUnitOfMeasurementQuery, ErrorOr<UnitOfMeasurementResult>>
{
    private readonly IUnitOfMeasurementRepository _unitOfMeasurementRepository;

    public GetByIdUnitOfMeasurementQueryHandler(IUnitOfMeasurementRepository unitOfMeasurementRepository)
    {
        _unitOfMeasurementRepository = unitOfMeasurementRepository;
    }

    public async Task<ErrorOr<UnitOfMeasurementResult>> Handle(
        GetByIdUnitOfMeasurementQuery query,
        CancellationToken cancellationToken)
    {
        var unitOfMeasurement = await _unitOfMeasurementRepository.GetByIdAsync(UnitOfMeasurementId.Create(query.Id));

        if (unitOfMeasurement is null)
            return Errors.UnitOfMeasurement.NotFound;

        return new UnitOfMeasurementResult(unitOfMeasurement.Id.Value, unitOfMeasurement.Name, unitOfMeasurement.IsActive);
    }
}