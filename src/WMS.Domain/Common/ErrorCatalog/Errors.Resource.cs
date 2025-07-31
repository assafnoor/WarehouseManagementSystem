using ErrorOr;

namespace WMS.Domain.Common.ErrorCatalog;

public static partial class Errors
{
    public static class Resource
    {
        public static Error NameAlreadyExists => Error.Conflict(
            code: "Resource.NameAlreadyExists",
            description: "A resource with the same name already exists.");

        public static Error NotFound => Error.NotFound(
            code: "Resource.NotFound",
            description: "A resource with Given Id Not Found.");

        public static Error AlreadyArchived => Error.Conflict(
            code: "Resource.AlreadyArchived",
            description: "The resource is already archived.");

        public static Error AlreadyActive => Error.Conflict(
            code: "Resource.AlreadyActive",
            description: "The resource is already active.");

        public static Error Archived => Error.Conflict(
            code: "Resource.Archived",
            description: "cant update resoucrs Archived.");
    }
}