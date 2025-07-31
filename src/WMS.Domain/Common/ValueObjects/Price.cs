using WMS.Domain.Common.Models;

namespace WMS.DomainCommon.ValueObjects;

public sealed class Price : ValueObject
{
    public decimal Amount { get; }
    public string Currency { get; }

    private Price(decimal amount, string currency)
    {
        Amount = amount;
        Currency = currency;
    }

    public static Price CreateNew(decimal amount, string currency)
    {
        return new Price(amount, currency);
    }

    public override IEnumerable<object> EqualityComponents
    {
        get
        {
            yield return Amount;
            yield return Currency;
        }
    }
}