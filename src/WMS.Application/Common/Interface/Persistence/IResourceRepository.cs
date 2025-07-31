using WMS.Domain.ResourceAggregate;

namespace MyApp.Application.Common.Interfaces.Persistance;

public interface IResourceRepository
{
    Task<Resource?> GetByIdAsync(Guid id);

    Task<Resource?> GetByNameAsync(string name);

    Task<bool> IsResourceUsedInDocumentsAsync(Guid resourceId);

    Task<IEnumerable<Resource>> GetActiveResourcesAsync();

    Task<IEnumerable<Resource>> GetArchivedResourcesAsync();

    Task<IEnumerable<Resource>> GetAllResourcesAsync();

    Task AddAsync(Resource resource);

    Task UpdateAsync(Resource resource);
}