using ErrorOr;
using MediatR;
using WMS.Application.UnitOfMeasurements.Common;

namespace WMS.Application.UnitOfMeasurements.Commands.Archive;

public record ArchiveUnitOfMeasurementCommand(
    Guid Id
) : IRequest<ErrorOr<UnitOfMeasurementResult>>;