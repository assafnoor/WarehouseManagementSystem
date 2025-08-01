using ErrorOr;
using MediatR;
using WMS.Application.ReceiptDocuments.Common;

namespace WMS.Application.ReceiptDocuments.Commands.Update;

public record UpdateReceiptDocumentCommand(
    Guid Id,
    string DocumentNumber,
    DateTime Date,
    List<UpdateReceiptResourceCommand>? ReceiptResources
) : IRequest<ErrorOr<ReceiptDocumentResult>>;

public record UpdateReceiptResourceCommand(
    Guid ResourceId,
    Guid UnitOfMeasurementId,
    decimal Quantity
    );