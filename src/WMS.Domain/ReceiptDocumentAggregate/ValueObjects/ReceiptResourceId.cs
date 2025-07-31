using WMS.Domain.Common.Models;

namespace WMS.Domain.ReceiptDocumentAggregate.ValueObjects;

public sealed class ReceiptResourceId : AggregateRootId<Guid>
{
    public override Guid Value { get; protected set; }

    private ReceiptResourceId(Guid value)
    {
        Value = value;
    }

    public static ReceiptResourceId CreateUnique()
    {
        return new(Guid.NewGuid());
    }

    public static ReceiptResourceId Create(Guid value)
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