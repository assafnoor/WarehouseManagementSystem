using WMS.Domain.Common.Models;
using WMS.Domain.UnitOfMeasurementAggregate.ValueObjects;

namespace WMS.Domain.UnitOfMeasurementAggregate;

public sealed class UnitOfMeasurement : AggregateRoot<UnitOfMeasurementId, Guid>
{
    public string Name { get; private set; }
    public bool IsActive { get; private set; }

    private UnitOfMeasurement(
        UnitOfMeasurementId id,
        string name)
        : base(id)
    {
        Name = name;
        IsActive = true;
    }

    public static UnitOfMeasurement Create(string name)
    {
        return new UnitOfMeasurement(
            UnitOfMeasurementId.CreateUnique(),
            name);
    }
}