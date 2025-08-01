namespace WMS.Contracts.ReceiptDocument;

public record ReceiptDocumentResponse(
    Guid Id,
    string DocumentNumber,
    DateTime Date,
    IReadOnlyList<ReceiptResourceResponse> ReceiptResources
);

public record ReceiptResourceResponse(
    Guid ResourceId,
    Guid UnitOfMeasurementId,
    decimal Quantity
);