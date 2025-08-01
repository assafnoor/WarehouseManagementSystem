using ErrorOr;
using System.Globalization;
using WMS.Domain.Common.ErrorCatalog;
using WMS.Domain.Common.Models;

namespace WMS.Domain.Common.ValueObjects;

public sealed class Quantity : ValueObject
{
    public decimal Value { get; private set; }

    private Quantity(decimal value)
    {
        // CA1062: Validate parameter 'other' is non-null
        if (value < 0)
            throw new ArgumentException("Quantity cannot be negative", nameof(value));

        Value = value;
    }

    public static ErrorOr<Quantity> CreateNew(decimal value)
    {
        if (value < 0)
            return Errors.Quantity.NegativeValue;

        return new Quantity(value);
    }

    public static Quantity Zero => new(0);

    public ErrorOr<Quantity> Add(Quantity other)
    {
        // CA1062: Validate parameter 'other' is non-null
        if (other is null)
        {
            throw new ArgumentNullException(nameof(other), "The 'other' quantity cannot be null.");
        }
        if (other.Value < 0)
            return Errors.Quantity.NegativeValue;

        return new Quantity(Value + other.Value);
    }

    public ErrorOr<Quantity> Subtract(Quantity other)
    {
        // CA1062: Validate parameter 'other' is non-null
        if (other is null)
        {
            throw new ArgumentNullException(nameof(other), "The 'other' quantity cannot be null.");
        }
        if (other.Value < 0 || Value < other.Value)
            return Errors.Quantity.NegativeValue;

        return new Quantity(Value - other.Value);
    }

    public bool IsGreaterThan(Quantity other)
    {
        // CA1062: Validate parameter 'other' is non-null
        if (other is null)
        {
            throw new ArgumentNullException(nameof(other), "The 'other' quantity cannot be null.");
        }

        return Value > other.Value;
    }

    public bool IsLessThan(Quantity other)
    {
        // CA1062: Validate parameter 'other' is non-null

        if (other is null)
        {
            throw new ArgumentNullException(nameof(other), "The 'other' quantity cannot be null.");
        }

        return Value < other.Value;
    }

    public bool IsZero => Value == 0;

    public override IEnumerable<object> EqualityComponents
    {
        get
        {
            yield return Value;
        }
    }

    public override string ToString()
    {
        // Use CultureInfo.InvariantCulture for consistent, culture-agnostic formatting.
        return Value.ToString("F2", CultureInfo.InvariantCulture);
    }
}