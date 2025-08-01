using WMS.Application.Common.Interface.Persistence;
using WMS.Domain.BalanceAggregate;
using WMS.Domain.ResourceAggregate.ValueObjects;
using WMS.Domain.UnitOfMeasurementAggregate.ValueObjects;

namespace WMS.Infrastructure.Persistence.Repositories;

public class BalanceRepository : IBalanceRepository
{
    private readonly MyAppDbContext _context;

    public BalanceRepository(MyAppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Balance balance, CancellationToken cancellationToken = default)
    {
        await _context.Balances.AddAsync(balance, cancellationToken);
    }

    public Task<Balance?> GetByResourceAndUnitAsync(ResourceId resourceId, UnitOfMeasurementId unitOfMeasurementId, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public void Remove(Balance balance)
    {
        throw new NotImplementedException();
    }

    public void Update(Balance balance)
    {
        throw new NotImplementedException();
    }
}