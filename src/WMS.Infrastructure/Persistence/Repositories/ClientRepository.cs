using Microsoft.EntityFrameworkCore;
using WMS.Application.Common.Interface.Persistence;
using WMS.Domain.ClientAggregate;

namespace WMS.Infrastructure.Persistence.Repositories;

public class ClientRepository : IClientRepository
{
    private readonly MyAppDbContext _context;

    public ClientRepository(MyAppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Client client)
    {
        await _context.Clients
            .AddAsync(client);
    }

    public Task<IEnumerable<Client>> GetActiveResourcesAsync()
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Client>> GetAllResourcesAsync()
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Client>> GetArchivedResourcesAsync()
    {
        throw new NotImplementedException();
    }

    public Task<Client?> GetByIdAsync(Guid clientId)
    {
        throw new NotImplementedException();
    }

    public async Task<Client?> GetByNameAsync(string name)
    {
        return await _context.Clients
            .FirstOrDefaultAsync(c => c.Name.ToLower() == name.ToLower());
    }

    public Task<bool> IsResourceUsedInDocumentsAsync(Guid clientId)
    {
        throw new NotImplementedException();
    }

    public Task UpdateAsync(Client resource)
    {
        throw new NotImplementedException();
    }
}