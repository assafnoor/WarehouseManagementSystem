using WMS.Domain.Common.Models;

namespace WMS.Domain.Common.ValueObjects;

public sealed class Address : ValueObject
{
    public string? Street { get; }
    public string? City { get; }
    public string? PostalCode { get; }

    private Address(string? street, string? city, string? postalCode)
    {
        Street = street;
        City = city;
        PostalCode = postalCode;
    }

    public static Address Create(string? street, string? city, string? postalCode = null)
    {
        return new Address(street, city, postalCode);
    }

    public override IEnumerable<object> EqualityComponents
    {
        get
        {
            yield return Street ?? string.Empty;
            yield return City ?? string.Empty;
            yield return PostalCode ?? string.Empty;
        }
    }
}