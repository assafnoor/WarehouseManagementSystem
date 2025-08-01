using WMS.Domain.BalanceAggregate;
using WMS.Domain.ResourceAggregate.ValueObjects;
using WMS.Domain.UnitOfMeasurementAggregate.ValueObjects;

namespace WMS.Application.Common.Interface.Persistence;

public interface IBalanceRepository
{
    Task<Balance?> GetByResourceAndUnitAsync(
            ResourceId resourceId,
            UnitOfMeasurementId unitOfMeasurementId,
            CancellationToken cancellationToken = default);

    Task AddAsync(Balance balance, CancellationToken cancellationToken = default);

    void Update(Balance balance);

    void Remove(Balance balance);
}