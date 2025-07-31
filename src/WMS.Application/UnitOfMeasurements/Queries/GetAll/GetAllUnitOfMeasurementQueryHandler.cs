using ErrorOr;
using MediatR;
using WMS.Application.Common.Interface.Persistence;
using WMS.Application.UnitOfMeasurements.Common;
using WMS.Domain.UnitOfMeasurementAggregate;

namespace WMS.Application.UnitOfMeasurements.Queries.GetAll;

public class GetAllUnitOfMeasurementQueryHandler :
    IRequestHandler<GetAllUnitOfMeasurementQuery, ErrorOr<IEnumerable<UnitOfMeasurementResult>>>
{
    private readonly IUnitOfMeasurementRepository _unitOfMeasurementRepository;

    public GetAllUnitOfMeasurementQueryHandler(IUnitOfMeasurementRepository unitOfMeasurementRepository)
    {
        _unitOfMeasurementRepository = unitOfMeasurementRepository;
    }

    public async Task<ErrorOr<IEnumerable<UnitOfMeasurementResult>>> Handle(
        GetAllUnitOfMeasurementQuery query,
        CancellationToken cancellationToken)
    {
        var unitOfMeasurement = await _unitOfMeasurementRepository.GetAllAsync(query.Status, query.Page, query.PageSize)
            ?? Array.Empty<UnitOfMeasurement>();

        var results = unitOfMeasurement
            .Select(c => new UnitOfMeasurementResult(c.Id.Value, c.Name, c.IsActive))
            .ToList();

        return results;
    }
}