using ErrorOr;
using MediatR;
using WMS.Application.UnitOfMeasurements.Common;

namespace WMS.Application.UnitOfMeasurements.Commands.Update;

public record UpdateUnitOfMeasurementCommand(
    Guid Id,
    string Name
) : IRequest<ErrorOr<UnitOfMeasurementResult>>;