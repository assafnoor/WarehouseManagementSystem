using WMS.Domain.Common.Models;

namespace WMS.Domain.ClientAggregate.ValueObjects;

public sealed class ClientId : AggregateRootId<Guid>
{
    public override Guid Value { get; protected set; }

    private ClientId(Guid value)
    {
        Value = value;
    }

    public static ClientId CreateUnique()
    {
        return new(Guid.NewGuid());
    }

    public static ClientId Create(Guid value)
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

#pragma warning disable CS8618

    private ClientId()
    { }

#pragma warning restore CS8618
}