using WMS.Application.Common.Interface.Persistence;
using WMS.Domain.UnitOfMeasurementAggregate;

namespace WMS.Infrastructure.Persistence.Repositories;

public class UnitOfMeasurementRepository : IUnitOfMeasurementRepository
{
    public Task AddAsync(UnitOfMeasurement unitOfMeasurement)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<UnitOfMeasurement>> GetActiveResourcesAsync()
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<UnitOfMeasurement>> GetAllResourcesAsync()
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<UnitOfMeasurement>> GetArchivedResourcesAsync()
    {
        throw new NotImplementedException();
    }

    public Task<UnitOfMeasurement?> GetByIdAsync(Guid unitOfMeasurementId)
    {
        throw new NotImplementedException();
    }

    public Task<UnitOfMeasurement?> GetByNameAsync(string name)
    {
        throw new NotImplementedException();
    }

    public Task<bool> IsResourceUsedInDocumentsAsync(Guid unitOfMeasurementId)
    {
        throw new NotImplementedException();
    }

    public Task UpdateAsync(UnitOfMeasurement unitOfMeasurement)
    {
        throw new NotImplementedException();
    }
}