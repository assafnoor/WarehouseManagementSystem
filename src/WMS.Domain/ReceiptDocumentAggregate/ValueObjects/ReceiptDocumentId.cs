using WMS.Domain.Common.Models;

namespace WMS.Domain.ReceiptDocumentAggregate.ValueObjects;

public sealed class ReceiptDocumentId : AggregateRootId<Guid>
{
    public override Guid Value { get; protected set; }

    private ReceiptDocumentId(Guid value)
    {
        Value = value;
    }

    public static ReceiptDocumentId CreateUnique()
    {
        return new(Guid.NewGuid());
    }

    public static ReceiptDocumentId Create(Guid value)
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