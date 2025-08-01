using ErrorOr;

namespace WMS.Domain.Common.ErrorCatalog;

public partial class Errors
{
    public static class Doamin
    {
        public static Error ResourceNotFound => Error.Validation(
           code: "Receipt.ResourceNotFound",
           description: "Resource not found in receipt.");
    }
}