using WMS.Domain.Common.ValueObjects;
using WMS.Domain.ReceiptDocumentAggregate;
using WMS.Domain.ReceiptDocumentAggregate.ValueObjects;

namespace WMS.Application.Common.Interface.Persistence;

public interface IReceiptDocumentRepository
{
    Task<bool> ExistsByNumberAsync(DocumentNumber number);

    Task<ReceiptDocument?> GetByIdAsync(ReceiptDocumentId receiptDocumentId);

    Task AddAsync(ReceiptDocument receiptDocument);

    Task<bool> ExistsByNumberAsync(DocumentNumber documentNumber, ReceiptDocumentId excludeId);

    void Update(ReceiptDocument receiptDocument);

    void Remove(ReceiptDocument receiptDocument);
}