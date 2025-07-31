using ErrorOr;

namespace WMS.Domain.Common.ErrorCatalog;

public static partial class Errors
{
    public static class Client
    {
        public static Error AlreadyArchived => Error.Conflict(
            code: "Client.AlreadyArchived",
            description: "The client is already archived.");

        public static Error AlreadyActive => Error.Conflict(
            code: "Client.AlreadyActive",
            description: "The client is already active.");
    }
}