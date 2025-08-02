using ErrorOr;
using MediatR;
using WMS.Application.ReceiptDocuments.Common;

namespace WMS.Application.ReceiptDocuments.Queries.GetAll;

public record GetAllReceiptDocumentQuery(
     DateTime? fromDate,
     DateTime? toDate,
     List<string>? documentNumbers,
     List<Guid>? resourceIds,
     List<Guid>? unitIds,
     int pageNumber = 1,
     int pageSize = 20
) : IRequest<ErrorOr<IEnumerable<ReceiptDocumentResult>>>;