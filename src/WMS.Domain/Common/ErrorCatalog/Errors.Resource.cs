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
            description: "No resource was found with the specified ID.");

        public static Error AlreadyArchived => Error.Conflict(
            code: "Resource.AlreadyArchived",
            description: "The resource is already archived.");

        public static Error AlreadyActive => Error.Conflict(
            code: "Resource.AlreadyActive",
            description: "The resource is already active.");

        public static Error Archived => Error.Conflict(
            code: "Resource.Archived",
            description: "Cannot update an archived resource.");

        public static Error CannotArchiveInUse => Error.Conflict(
            code: "Resource.CannotArchiveInUse",
            description: "Resource is currently in use and cannot be archived.");
    }
}