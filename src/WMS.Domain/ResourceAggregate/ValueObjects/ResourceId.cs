using WMS.Domain.Common.Models;

namespace WMS.Domain.ResourceAggregate.ValueObjects;

public sealed class ResourceId : AggregateRootId<Guid>
{
    public override Guid Value { get; protected set; }

    private ResourceId(Guid value)
    {
        Value = value;
    }

    public static ResourceId CreateUnique()
    {
        return new(Guid.NewGuid());
    }

    public static ResourceId Create(Guid value)
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