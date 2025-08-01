using ErrorOr;
using MediatR;
using WMS.Application.Common.Interface.Persistence;
using WMS.Application.ReceiptDocuments.Common;
using WMS.Domain.Common.ErrorCatalog;
using WMS.Domain.ReceiptDocumentAggregate.ValueObjects;

namespace WMS.Application.ReceiptDocuments.Commands.Delete;

public class DeleteReceiptDocumentCommandHandler :
    IRequestHandler<DeleteReceiptDocumentCommand, ErrorOr<ReceiptDocumentResult>>
{
    private readonly IReceiptDocumentRepository _receiptDocumentRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteReceiptDocumentCommandHandler(IReceiptDocumentRepository receiptDocumentRepository, IUnitOfWork unitOfWork)
    {
        _receiptDocumentRepository = receiptDocumentRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ErrorOr<ReceiptDocumentResult>> Handle(
        DeleteReceiptDocumentCommand command,
        CancellationToken cancellationToken)
    {
        var receiptDocument = await _receiptDocumentRepository.GetByIdAsync(ReceiptDocumentId.Create(command.Id));

        if (receiptDocument is null)
        {
            return Errors.ReceiptDocument.NotFound;
        }
        receiptDocument.MarkAsDeleted();

        _receiptDocumentRepository.Remove(receiptDocument);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return new ReceiptDocumentResult(
            receiptDocument.Id.Value,
            receiptDocument.Number.Value,
            receiptDocument.Date,
            receiptDocument.ReceiptResources.Select(r => new ReceiptResourceResult(
                r.ResourceId.Value,
                r.UnitOfMeasurementId.Value,
                r.Quantity.Value
            )).ToList());
    }
}