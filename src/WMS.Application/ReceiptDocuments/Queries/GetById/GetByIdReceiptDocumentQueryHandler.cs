using ErrorOr;
using MediatR;
using WMS.Application.Common.Interface.Persistence;
using WMS.Application.ReceiptDocuments.Common;
using WMS.Domain.Common.ErrorCatalog;
using WMS.Domain.ReceiptDocumentAggregate.ValueObjects;

namespace WMS.Application.ReceiptDocuments.Queries.GetById;

public class GetByIdReceiptDocumentQueryHandler :
    IRequestHandler<GetByIdReceiptDocumentQuery, ErrorOr<ReceiptDocumentResult>>
{
    private readonly IReceiptDocumentRepository _receiptDocumentRepository;

    public GetByIdReceiptDocumentQueryHandler(IReceiptDocumentRepository receiptDocumentRepository)
    {
        _receiptDocumentRepository = receiptDocumentRepository;
    }

    public async Task<ErrorOr<ReceiptDocumentResult>> Handle(
        GetByIdReceiptDocumentQuery query,
        CancellationToken cancellationToken)
    {
        var receiptDocument = await _receiptDocumentRepository.GetByIdAsync(ReceiptDocumentId.Create(query.Id));

        if (receiptDocument is null)
            return Errors.ReceiptDocument.NotFound;

        return new ReceiptDocumentResult(
            receiptDocument.Id.Value,
            receiptDocument.Number.Value,
            receiptDocument.Date,
            receiptDocument.ReceiptResources.Select(r => new ReceiptResourceResult(
                r.ResourceId.Value,
                r.UnitOfMeasurementId.Value,
                r.Quantity.Value
            )).ToList()
        );
    }
}