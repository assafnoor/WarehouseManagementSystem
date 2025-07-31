using ErrorOr;

namespace WMS.Domain.Common.ErrorCatalog;

public static partial class Errors
{
    public static class UnitOfMeasurement
    {
        public static Error NameAlreadyExists => Error.Conflict(
            code: "UnitOfMeasurement.NameAlreadyExists",
            description: "A unit of measurement with the same name already exists.");

        public static Error NotFound => Error.NotFound(
            code: "UnitOfMeasurement.NotFound",
            description: "No unit of measurement was found with the specified ID.");

        public static Error AlreadyArchived => Error.Conflict(
            code: "UnitOfMeasurement.AlreadyArchived",
            description: "The unit of measurement is already archived.");

        public static Error AlreadyActive => Error.Conflict(
            code: "UnitOfMeasurement.AlreadyActive",
            description: "The unit of measurement is already active.");

        public static Error Archived => Error.Conflict(
            code: "UnitOfMeasurement.Archived",
            description: "Cannot update an archived unit of measurement.");

        public static Error CannotArchiveInUse => Error.Conflict(
            code: "UnitOfMeasurement.CannotArchiveInUse",
            description: "The unit of measurement is currently in use and cannot be archived.");
    }
}