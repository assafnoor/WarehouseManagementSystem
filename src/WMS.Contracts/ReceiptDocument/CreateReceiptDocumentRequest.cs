namespace WMS.Contracts.ReceiptDocument;

public record AddReceiptDocumentRequest(
  string DocumentNumber,
  DateTime Date,
  List<AddReceiptResourceRequest> ReceiptResources);

public record AddReceiptResourceRequest(
  Guid ResourceId,
  Guid UnitOfMeasurementId,
  decimal Quantity);