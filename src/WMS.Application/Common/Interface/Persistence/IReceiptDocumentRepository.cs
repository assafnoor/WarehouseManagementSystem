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

    public Task<IEnumerable<ReceiptDocument>> GetAllAsync(
       DateTime? fromDate,
 DateTime? toDate,
 List<string>? documentNumbers,
 List<Guid>? resourceIds,
 List<Guid>? unitIds,
  int pageNumber = 1,
  int pageSize = 20);
}