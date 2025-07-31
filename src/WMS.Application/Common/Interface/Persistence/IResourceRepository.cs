using WMS.Domain.ResourceAggregate;

namespace WMS.Application.Common.Interfaces.Persistence;

public interface IResourceRepository
{
    Task<Resource?> GetByIdAsync(Guid resourceId);

    Task<Resource?> GetByNameAsync(string name);

    Task<bool> IsResourceUsedInDocumentsAsync(Guid resourceId);

    Task<IEnumerable<Resource>> GetActiveResourcesAsync();

    Task<IEnumerable<Resource>> GetArchivedResourcesAsync();

    Task<IEnumerable<Resource>> GetAllResourcesAsync();

    Task AddAsync(Resource resource);

    Task UpdateAsync(Resource resource);

    Task<IEnumerable<Resource?>> GetAllAsync(
    bool? Status,
    int Page,
    int PageSize);
}