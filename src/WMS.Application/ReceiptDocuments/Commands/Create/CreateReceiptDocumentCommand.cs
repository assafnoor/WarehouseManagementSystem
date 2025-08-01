using ErrorOr;
using MediatR;
using WMS.Application.ReceiptDocuments.Common;

namespace WMS.Application.ReceiptDocuments.Commands.Create;

public record CreateReceiptDocumentCommand(
    string DocumentNumber,
    DateTime Date,
    List<ReceiptResourceCommand> ReceiptResources
) : IRequest<ErrorOr<ReceiptDocumentResult>>;

public record ReceiptResourceCommand(
    Guid ResourceId,
    Guid UnitOfMeasurementId,
    decimal Quantity
    );