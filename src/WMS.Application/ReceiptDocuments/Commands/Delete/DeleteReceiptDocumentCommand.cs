using ErrorOr;
using MediatR;
using WMS.Application.ReceiptDocuments.Common;

namespace WMS.Application.ReceiptDocuments.Commands.Delete;

public record DeleteReceiptDocumentCommand(
    Guid Id
) : IRequest<ErrorOr<ReceiptDocumentResult>>;