using ErrorOr;
using MediatR;
using WMS.Application.ReceiptDocuments.Common;

namespace WMS.Application.ReceiptDocuments.Queries.GetById;

public record GetByIdReceiptDocumentQuery(
    Guid Id
) : IRequest<ErrorOr<ReceiptDocumentResult>>;
