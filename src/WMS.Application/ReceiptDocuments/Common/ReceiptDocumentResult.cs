namespace WMS.Application.ReceiptDocuments.Common;

public record ReceiptDocumentResult(
    Guid Id,
    string DocumentNumber,
    DateTime Date,
    IReadOnlyList<ReceiptResourceResult> ReceiptResources
);

public record ReceiptResourceResult(
    Guid ResourceId,
    Guid UnitOfMeasurementId,
    decimal Quantity
);