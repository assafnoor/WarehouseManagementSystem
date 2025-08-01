using WMS.Domain.UnitOfMeasurementAggregate;
using WMS.Domain.UnitOfMeasurementAggregate.ValueObjects;

namespace WMS.Application.Common.Interface.Persistence;

public interface IUnitOfMeasurementRepository
{
    Task<UnitOfMeasurement?> GetByIdAsync(UnitOfMeasurementId unitOfMeasurementId);

    Task<UnitOfMeasurement?> GetByNameAsync(string name);

    Task<bool> IsUsedInDocumentsAsync(UnitOfMeasurementId unitOfMeasurementId);

    Task<IEnumerable<UnitOfMeasurement>> GetActiveAsync();

    Task<IEnumerable<UnitOfMeasurement>> GetArchivedAsync();

    Task<IEnumerable<UnitOfMeasurement>> GetAllAsync();

    Task AddAsync(UnitOfMeasurement unitOfMeasurement);

    Task UpdateAsync(UnitOfMeasurement unitOfMeasurement);

    Task<IEnumerable<UnitOfMeasurement?>> GetAllAsync(
        bool? Status,
        int Page,
        int PageSize);

    Task<bool> ExistsActiveAsync(UnitOfMeasurementId unitOfMeasurementId);
}