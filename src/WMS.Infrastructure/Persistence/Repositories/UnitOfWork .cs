using WMS.Application.Common.Interface.Persistence;

namespace WMS.Infrastructure.Persistence.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly MyAppDbContext _myAppDbContext;

    public UnitOfWork(MyAppDbContext myAppDbContext)
    {
        _myAppDbContext = myAppDbContext;
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return _myAppDbContext.SaveChangesAsync(cancellationToken);
    }
}