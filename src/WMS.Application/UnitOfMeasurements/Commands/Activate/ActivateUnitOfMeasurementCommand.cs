using ErrorOr;
using MediatR;
using WMS.Application.UnitOfMeasurements.Common;

namespace WMS.Application.UnitOfMeasurements.Commands.Activate;

public record ActivateUnitOfMeasurementCommand(
    Guid Id
) : IRequest<ErrorOr<UnitOfMeasurementResult>>;