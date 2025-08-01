using ErrorOr;

namespace WMS.Domain.Common.ErrorCatalog;

public partial class Errors
{
    public static class ReceiptDocument
    {
        public static Error DuplicateNumber => Error.Validation(
           code: "ReceiptDocument.DuplicateNumber",
           description: "A receipt document with the same number already exists.");

        public static Error InvalidResource => Error.Validation(
           code: "Resource.Invalid",
           description: "One or more resources or units of measurement are invalid or archived.");
    }
}