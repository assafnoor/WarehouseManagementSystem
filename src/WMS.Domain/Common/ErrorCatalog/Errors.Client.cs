using ErrorOr;

namespace WMS.Domain.Common.ErrorCatalog;

public static partial class Errors
{
    public static class Client
    {
        public static Error NameAlreadyExists => Error.Conflict(
            code: "Client.NameAlreadyExists",
            description: "A client with the same name already exists.");

        public static Error NotFound => Error.NotFound(
            code: "Client.NotFound",
            description: "The specified client was not found.");

        public static Error AlreadyArchived => Error.Conflict(
            code: "Client.AlreadyArchived",
            description: "The client is already archived.");

        public static Error AlreadyActive => Error.Conflict(
            code: "Client.AlreadyActive",
            description: "The client is already active.");

        public static Error Archived => Error.Conflict(
            code: "Client.Archived",
            description: "Cannot update an archived client.");

        public static Error CannotArchiveInUse => Error.Conflict(
            code: "Client.CannotArchiveInUse",
            description: "The client is used in outbound documents and cannot be archived.");
    }
}