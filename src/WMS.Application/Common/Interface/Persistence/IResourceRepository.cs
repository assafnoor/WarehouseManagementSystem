using WMS.Domain.ResourceAggregate;

namespace MyApp.Application.Common.Interfaces.Persistance;

public interface IResourceRepository
{
    Task AddAsync(Resource resource);

    Task<Resource?> GetByNameAsync(string name);
}