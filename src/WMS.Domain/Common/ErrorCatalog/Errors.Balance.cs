using ErrorOr;

namespace WMS.Domain.Common.ErrorCatalog;

public static partial class Errors
{
    public static class ErrorsBalance
    {
        public static Error InvalidQuantity => Error.Validation(
            code: "Balance.InvalidQuantity",
            description: "Quantity must be positive.");

        public static Error InsufficientQuantity => Error.Validation(
            code: "Balance.InsufficientQuantity",
            description: "Insufficient quantity in stock.");
    }
}