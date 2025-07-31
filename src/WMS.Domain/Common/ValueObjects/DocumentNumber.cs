using WMS.Domain.Common.Models;

namespace WMS.Domain.Common.ValueObjects;

public sealed class DocumentNumber : ValueObject
{
    public string Value { get; private set; }

    private DocumentNumber(string value)
    {
        Value = value;
    }

    public static DocumentNumber CreateNew(string value) => new(value);

    public override IEnumerable<object> EqualityComponents
    {
        get
        {
            yield return Value;
        }
    }

    public override string ToString() => Value;
}