using ErrorOr;

namespace WMS.Domain.Common.ErrorCatalog;

public static partial class Errors
{
    public static class Quantity
    {
        public static Error NegativeValue => Error.Validation(
           code: "Quantity.NegativeValue",
           description: "Quantity cannot be negative.");
    }
}