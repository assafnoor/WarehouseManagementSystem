using ErrorOr;
using MediatR;
using WMS.Application.UnitOfMeasurements.Common;

namespace WMS.Application.UnitOfMeasurements.Commands.Create;

public record CreateUnitOfMeasurementCommand(
    string Name
) : IRequest<ErrorOr<UnitOfMeasurementResult>>;