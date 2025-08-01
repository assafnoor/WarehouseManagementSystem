namespace WMS.Contracts.ReceiptDocument;

public record UpdateReceiptDocumentRequest(
    Guid Id,
  string DocumentNumber,
  DateTime Date,
  List<AddReceiptResourceRequest>? ReceiptResources);

public record UpdateReceiptResourceRequest(
  Guid ResourceId,
  Guid UnitOfMeasurementId,
  decimal Quantity);