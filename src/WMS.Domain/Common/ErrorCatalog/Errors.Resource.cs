using ErrorOr;

namespace WMS.Domain.Common.ErrorCatalog;

public static partial class Errors
{
    public static class Resource
    {
        public static Error NameAlreadyExists => Error.Conflict(
            code: "Resource.NameAlreadyExists",
            description: "A resource with the same name already exists.");
    }
}