using ErrorOr;
using MediatR;
using WMS.Application.Balances.Common;

namespace WMS.Application.Balances.Commands.Create;

public record CreateBalanceCommand(
    Guid ResourceId,
    Guid UnitOfMeasurementId,
    decimal Quantity
) : IRequest<ErrorOr<BalanceResult>>;