using WMS.Domain.ClientAggregate;
using WMS.Domain.ClientAggregate.ValueObjects;

namespace WMS.Application.Common.Interface.Persistence;

public interface IClientRepository
{
    Task<Client?> GetByIdAsync(ClientId clientId);

    Task<Client?> GetByNameAsync(string name);

    Task<bool> IsUsedInDocumentsAsync(ClientId clientId);

    Task<IEnumerable<Client>> GetActiveAsync();

    Task<IEnumerable<Client>> GetArchivedAsync();

    Task<IEnumerable<Client>> GetAllAsync();

    Task<IEnumerable<Client?>> GetAllAsync(
        bool? Status,
        int Page,
        int PageSize);

    Task AddAsync(Client client);

    Task UpdateAsync(Client client);
}