using WMS.Domain.Common.Models;

namespace WMS.Domain.ShipmentDocumentAggregate.ValueObjects;

public sealed class ShipmentDocumentId : AggregateRootId<Guid>
{
    public override Guid Value { get; protected set; }

    private ShipmentDocumentId(Guid value)
    {
        Value = value;
    }

    public static ShipmentDocumentId CreateUnique()
    {
        return new(Guid.NewGuid());
    }

    public static ShipmentDocumentId Create(Guid value)
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

    private ShipmentDocumentId()
    { }
}