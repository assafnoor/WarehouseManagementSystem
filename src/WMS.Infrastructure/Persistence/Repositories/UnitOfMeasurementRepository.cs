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

    public Task<IEnumerable<UnitOfMeasurement>> GetActiveAsync()
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<UnitOfMeasurement>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<UnitOfMeasurement>> GetArchivedAsync()
    {
        throw new NotImplementedException();
    }

    public async Task<UnitOfMeasurement?> GetByIdAsync(UnitOfMeasurementId unitOfMeasurementId)
    {
        return await _context.UnitOfMeasurements
            .FirstOrDefaultAsync(c => c.Id == unitOfMeasurementId);
    }

    public async Task<UnitOfMeasurement?> GetByNameAsync(string name)
    {
        return await _context.UnitOfMeasurements
            .FirstOrDefaultAsync(c => c.Name.ToLower() == name.ToLower());
    }

    public Task<bool> IsUsedInDocumentsAsync(UnitOfMeasurementId unitOfMeasurementId)
    {
        throw new NotImplementedException();
    }

    public Task UpdateAsync(UnitOfMeasurement unitOfMeasurement)
    {
        _context.UnitOfMeasurements.Update(unitOfMeasurement);
        return Task.CompletedTask;
    }

    public async Task<IEnumerable<UnitOfMeasurement?>> GetAllAsync(bool? status, int page, int pageSize)
    {
        var query = _context.UnitOfMeasurements.AsQueryable();

        // Filter by status if provided
        if (status.HasValue)
        {
            query = query.Where(c => c.IsActive == status.Value);
        }

        // Apply pagination
        var unitOfMeasurements = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return unitOfMeasurements;
    }

    public async Task<bool> ExistsActiveAsync(UnitOfMeasurementId unitOfMeasurementId)
    {
        return await _context.UnitOfMeasurements
            .AnyAsync(r => r.Id == unitOfMeasurementId && r.IsActive);
    }
}