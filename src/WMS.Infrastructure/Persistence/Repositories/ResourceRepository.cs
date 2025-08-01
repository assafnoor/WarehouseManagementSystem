using Microsoft.EntityFrameworkCore;
using WMS.Application.Common.Interface.Persistence;
using WMS.Domain.ResourceAggregate;
using WMS.Domain.ResourceAggregate.ValueObjects;

namespace WMS.Infrastructure.Persistence.Repositories;

public sealed class ResourceRepository : IResourceRepository
{
    private readonly MyAppDbContext _context;

    public ResourceRepository(MyAppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Resource resource)
    {
        await _context.Resources
            .AddAsync(resource);
    }

    public Task<IEnumerable<Resource>> GetActiveResourcesAsync()
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Resource>> GetAllResourcesAsync()
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Resource>> GetArchivedResourcesAsync()
    {
        throw new NotImplementedException();
    }

    public async Task<Resource?> GetByIdAsync(Guid resourceId)
    {
        var id = ResourceId.Create(resourceId);
        return await _context.Resources
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<Resource?> GetByNameAsync(string name)
    {
        return await _context.Resources
            .FirstOrDefaultAsync(c => c.Name.ToLower() == name.ToLower());
    }

    public Task<bool> IsResourceUsedInDocumentsAsync(Guid resourceId)
    {
        throw new NotImplementedException();
    }

    public Task UpdateAsync(Resource resource)
    {
        _context.Resources.Update(resource);
        return Task.CompletedTask;
    }

    public async Task<IEnumerable<Resource?>> GetAllAsync(bool? status, int page, int pageSize)
    {
        var query = _context.Resources.AsQueryable();

        // Filter by status if provided
        if (status.HasValue)
        {
            query = query.Where(c => c.IsActive == status.Value);
        }

        // Apply pagination
        var resources = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return resources;
    }
}