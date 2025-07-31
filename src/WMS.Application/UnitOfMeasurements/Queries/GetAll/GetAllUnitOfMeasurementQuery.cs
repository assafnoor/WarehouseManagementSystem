using ErrorOr;
using MediatR;
using WMS.Application.UnitOfMeasurements.Common;

namespace WMS.Application.UnitOfMeasurements.Queries.GetAll;

public record GetAllUnitOfMeasurementQuery(
    bool? Status,
    int Page,
    int PageSize
) : IRequest<ErrorOr<IEnumerable<UnitOfMeasurementResult>>>;