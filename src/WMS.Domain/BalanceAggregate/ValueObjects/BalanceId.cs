using WMS.Domain.Common.Models;

namespace WMS.Domain.BalanceAggregate.ValueObjects;

public sealed class BalanceId : AggregateRootId<Guid>
{
    public override Guid Value { get; protected set; }

    private BalanceId(Guid value)
    {
        Value = value;
    }

    public static BalanceId CreateUnique()
    {
        return new(Guid.NewGuid());
    }

    public static BalanceId Create(Guid value)
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

    private BalanceId()
    { }

#pragma warning restore CS8618
}