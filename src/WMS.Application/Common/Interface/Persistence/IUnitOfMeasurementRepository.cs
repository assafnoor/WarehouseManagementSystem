using WMS.Domain.UnitOfMeasurementAggregate;

namespace WMS.Application.Common.Interface.Persistence;

public interface IUnitOfMeasurementRepository
{
    Task<UnitOfMeasurement?> GetByIdAsync(Guid unitOfMeasurementId);

    Task<UnitOfMeasurement?> GetByNameAsync(string name);

    Task<bool> IsResourceUsedInDocumentsAsync(Guid unitOfMeasurementId);

    Task<IEnumerable<UnitOfMeasurement>> GetActiveResourcesAsync();

    Task<IEnumerable<UnitOfMeasurement>> GetArchivedResourcesAsync();

    Task<IEnumerable<UnitOfMeasurement>> GetAllResourcesAsync();

    Task AddAsync(UnitOfMeasurement unitOfMeasurement);

    Task UpdateAsync(UnitOfMeasurement unitOfMeasurement);

    Task<IEnumerable<UnitOfMeasurement?>> GetAllAsync(
        bool? Status,
        int Page,
        int PageSize);

    Task<bool> ExistsActiveAsync(Guid unitOfMeasurementId);
}