using ErrorOr;
using MediatR;
using WMS.Application.Common.Interface.Persistence;
using WMS.Application.ReceiptDocuments.Common;
using WMS.Domain.Common.ErrorCatalog;

namespace WMS.Application.ReceiptDocuments.Queries.GetAll;

public class GetAllReceiptDocumentQueryHandler :
    IRequestHandler<GetAllReceiptDocumentQuery, ErrorOr<IEnumerable<ReceiptDocumentResult>>>
{
    private readonly IReceiptDocumentRepository _receiptDocumentRepository;

    public GetAllReceiptDocumentQueryHandler(IReceiptDocumentRepository receiptDocumentRepository)
    {
        _receiptDocumentRepository = receiptDocumentRepository;
    }

    public async Task<ErrorOr<IEnumerable<ReceiptDocumentResult>>> Handle(
        GetAllReceiptDocumentQuery query,
        CancellationToken cancellationToken)
    {
        var receiptDocuments = await _receiptDocumentRepository.GetAllAsync(
            query.fromDate,
            query.toDate,
            query.documentNumbers,
            query.resourceIds,
            query.unitIds,
            query.pageNumber,
            query.pageSize);

        if (receiptDocuments == null || !receiptDocuments.Any())
            return Errors.ReceiptDocument.NotFound;

        var results = receiptDocuments.Select(rd =>
            new ReceiptDocumentResult(
                rd.Id.Value,
                rd.Number.Value,
                rd.Date,
                rd.ReceiptResources.Select(rr => new ReceiptResourceResult(
                    rr.ResourceId.Value,
                    rr.UnitOfMeasurementId.Value,
                    rr.Quantity.Value)).ToList()
            )).ToList();

        return results;
    }
}