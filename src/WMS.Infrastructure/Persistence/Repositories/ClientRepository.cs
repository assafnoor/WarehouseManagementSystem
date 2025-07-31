using Microsoft.EntityFrameworkCore;
using WMS.Application.Common.Interface.Persistence;
using WMS.Domain.ClientAggregate;
using WMS.Domain.ClientAggregate.ValueObjects;

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

    public async Task<IEnumerable<Client>> GetActiveResourcesAsync()
    {
        return await _context.Clients
            .Where(c => c.IsActive)
            .ToListAsync();
    }

    public async Task<IEnumerable<Client?>> GetAllAsync(bool? status, int page, int pageSize)
    {
        var query = _context.Clients.AsQueryable();

        // Filter by status if provided
        if (status.HasValue)
        {
            query = query.Where(c => c.IsActive == status.Value);
        }

        // Apply pagination
        var clients = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return clients;
    }

    public async Task<IEnumerable<Client>> GetAllResourcesAsync()
    {
        return await _context.Clients
            .ToListAsync();
    }

    public async Task<IEnumerable<Client>> GetArchivedResourcesAsync()
    {
        return await _context.Clients
            .Where(c => !c.IsActive)
            .ToListAsync();
    }

    public async Task<Client?> GetByIdAsync(Guid clientId)
    {
        var id = ClientId.Create(clientId);
        return await _context.Clients
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<Client?> GetByNameAsync(string name)
    {
        return await _context.Clients
            .FirstOrDefaultAsync(c => c.Name.ToLower() == name.ToLower());
    }

    public async Task<bool> IsResourceUsedInDocumentsAsync(Guid clientId)
    {
        // Placeholder: Replace with actual document check
        return await Task.FromResult(false);
    }

    public Task UpdateAsync(Client client)
    {
        _context.Clients.Update(client);
        return Task.CompletedTask;
    }
}