using ErrorOr;
using MediatR;
using WMS.Application.UnitOfMeasurements.Common;

namespace WMS.Application.UnitOfMeasurements.Queries.GetById;

public record GetByIdUnitOfMeasurementQuery(
    Guid Id
) : IRequest<ErrorOr<UnitOfMeasurementResult>>;
