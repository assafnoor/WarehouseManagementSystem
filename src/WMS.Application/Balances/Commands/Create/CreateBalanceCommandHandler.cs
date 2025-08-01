using ErrorOr;
using MediatR;
using WMS.Application.Balances.Common;
using WMS.Application.Common.Interface.Persistence;
using WMS.Domain.Common.ValueObjects;
using WMS.Domain.ResourceAggregate.ValueObjects;
using WMS.Domain.UnitOfMeasurementAggregate.ValueObjects;

namespace WMS.Application.Balances.Commands.Create;

public class CreateBalanceCommandHandler :
    IRequestHandler<CreateBalanceCommand, ErrorOr<BalanceResult>>
{
    private readonly IBalanceRepository _balanceRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateBalanceCommandHandler(IBalanceRepository balanceRepository, IUnitOfWork unitOfWork)
    {
        _balanceRepository = balanceRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ErrorOr<BalanceResult>> Handle(
        CreateBalanceCommand command,
        CancellationToken cancellationToken)
    {
        var resourceId = ResourceId.Create(command.ResourceId);
        var unitId = UnitOfMeasurementId.Create(command.UnitOfMeasurementId);
        var quantity = Quantity.CreateNew(command.Quantity);
        // to do
        return default;
    }
}