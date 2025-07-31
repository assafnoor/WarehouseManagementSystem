using WMS.Domain.Common.Models;

namespace WMS.Domain.UnitOfMeasurementAggregate.ValueObjects;

public sealed class UnitOfMeasurementId : AggregateRootId<Guid>
{
    public override Guid Value { get; protected set; }

    private UnitOfMeasurementId(Guid value)
    {
        Value = value;
    }

    public static UnitOfMeasurementId CreateUnique()
    {
        return new(Guid.NewGuid());
    }

    public static UnitOfMeasurementId Create(Guid value)
    {
        return new(value);
    }

    public override IEnumerable<object> EqualityComponents
    {
        get
        {
            yield return Value;
        }
    }
}