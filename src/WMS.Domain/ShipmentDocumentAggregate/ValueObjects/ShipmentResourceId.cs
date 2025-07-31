using WMS.Domain.Common.Models;

namespace WMS.Domain.ShipmentDocumentAggregate.ValueObjects;

public sealed class ShipmentResourceId : AggregateRootId<Guid>
{
    public override Guid Value { get; protected set; }

    private ShipmentResourceId(Guid value) => Value = value;

    public static ShipmentResourceId CreateUnique() => new(Guid.NewGuid());

    public static ShipmentResourceId Create(Guid value) => new(value);

    public override IEnumerable<object> EqualityComponents
    {
        get
        {
            yield return Value;
        }
    }

    private ShipmentResourceId()
    { }
}