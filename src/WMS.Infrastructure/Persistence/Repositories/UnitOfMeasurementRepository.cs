using Microsoft.EntityFrameworkCore;
using WMS.Application.Common.Interface.Persistence;
using WMS.Domain.UnitOfMeasurementAggregate;
using WMS.Domain.UnitOfMeasurementAggregate.ValueObjects;

namespace WMS.Infrastructure.Persistence.Repositories;

public class UnitOfMeasurementRepository : IUnitOfMeasurementRepository
{
    private readonly MyAppDbContext _context;

    public UnitOfMeasurementRepository(MyAppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(UnitOfMeasurement unitOfMeasurement)
    {
        await _context.UnitOfMeasurements
            .AddAsync(unitOfMeasurement);
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

    public async Task<UnitOfMeasurement?> GetByIdAsync(Guid unitOfMeasurementId)
    {
        var id = UnitOfMeasurementId.Create(unitOfMeasurementId);
        return await _context.UnitOfMeasurements
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<UnitOfMeasurement?> GetByNameAsync(string name)
    {
        return await _context.UnitOfMeasurements
            .FirstOrDefaultAsync(c => c.Name.ToLower() == name.ToLower());
    }

    public Task<bool> IsResourceUsedInDocumentsAsync(Guid unitOfMeasurementId)
    {
        throw new NotImplementedException();
    }

    public Task UpdateAsync(UnitOfMeasurement unitOfMeasurement)
    {
        _context.UnitOfMeasurements.Update(unitOfMeasurement);
        return Task.CompletedTask;
    }
}