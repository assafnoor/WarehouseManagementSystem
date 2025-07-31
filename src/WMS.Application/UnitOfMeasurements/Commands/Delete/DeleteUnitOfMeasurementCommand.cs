using ErrorOr;
using MediatR;
using WMS.Application.UnitOfMeasurements.Common;

namespace WMS.Application.UnitOfMeasurements.Commands.Delete;

public record DeleteUnitOfMeasurementCommand(
    Guid Id
) : IRequest<ErrorOr<UnitOfMeasurementResult>>;