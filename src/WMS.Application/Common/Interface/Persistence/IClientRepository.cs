using WMS.Domain.ClientAggregate;

namespace WMS.Application.Common.Interface.Persistence;

public interface IClientRepository
{
    Task<Client?> GetByIdAsync(Guid clientId);

    Task<Client?> GetByNameAsync(string name);

    Task<bool> IsResourceUsedInDocumentsAsync(Guid clientId);

    Task<IEnumerable<Client>> GetActiveResourcesAsync();

    Task<IEnumerable<Client>> GetArchivedResourcesAsync();

    Task<IEnumerable<Client>> GetAllResourcesAsync();

    Task<IEnumerable<Client?>> GetAllAsync(
        bool? Status,
        int Page,
        int PageSize);

    Task AddAsync(Client resource);

    Task UpdateAsync(Client resource);
}