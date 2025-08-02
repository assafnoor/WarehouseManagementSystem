namespace WMS.Contracts.ReceiptDocument;

public record GetAllReceiptDocumentRequest
(
    DateTime? fromDate,
     DateTime? toDate,
     List<string>? documentNumbers,
     List<Guid>? resourceIds,
     List<Guid>? unitIds,
     int pageNumber = 1,
     int pageSize = 20
);