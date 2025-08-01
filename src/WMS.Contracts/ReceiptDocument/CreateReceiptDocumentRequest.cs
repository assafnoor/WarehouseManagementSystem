namespace WMS.Contracts.ReceiptDocument;

public record CreateReceiptDocumentRequest(
  string DocumentNumber,
  DateTime Date,
  List<ReceiptResourceRequest> ReceiptResources);

public record ReceiptResourceRequest(
  Guid ResourceId,
  Guid UnitOfMeasurementId,
  decimal Quantity);