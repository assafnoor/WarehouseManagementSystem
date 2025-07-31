using MyApp.Application.Common.Interfaces.Persistence;
using WMS.Domain.ResourceAggregate;
using WMS.Infrastructure.Persistence;

namespace MyApp.Infrastructure.Persistence.Repositories;

public sealed class ResourceRepository : IResourceRepository
{
    private readonly MyAppDbContext _context;

    public ResourceRepository(MyAppDbContext context)
    {
        _context = context;
    }

    public Task AddAsync(Resource resource)
    {
        throw new NotImplementedException();
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

    public Task<Resource?> GetByIdAsync(Guid resourceId)
    {
        throw new NotImplementedException();
    }

    public Task<Resource?> GetByNameAsync(string name)
    {
        throw new NotImplementedException();
    }

    public Task<bool> IsResourceUsedInDocumentsAsync(Guid resourceId)
    {
        throw new NotImplementedException();
    }

    public Task UpdateAsync(Resource resource)
    {
        throw new NotImplementedException();
    }
}