using WMS.Domain.ResourceAggregate;
using WMS.Domain.ResourceAggregate.ValueObjects;

namespace WMS.Application.Common.Interface.Persistence;

public interface IResourceRepository
{
    Task<Resource?> GetByIdAsync(ResourceId resourceId);

    Task<Resource?> GetByNameAsync(string name);

    Task<bool> IsUsedInDocumentsAsync(ResourceId resourceId);

    Task<IEnumerable<Resource>> GetActiveAsync();

    Task<IEnumerable<Resource>> GetArchivedAsync();

    Task<IEnumerable<Resource>> GetAllAsync();

    Task AddAsync(Resource resource);

    Task UpdateAsync(Resource resource);

    Task<IEnumerable<Resource?>> GetAllAsync(
    bool? Status,
    int Page,
    int PageSize);

    Task<bool> ExistsActiveAsync(ResourceId resourceId);
}