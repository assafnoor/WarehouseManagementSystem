using WMS.Domain.Common.ValueObjects;
using WMS.Domain.ReceiptDocumentAggregate;

namespace WMS.Application.Common.Interface.Persistence;

public interface IReceiptDocumentRepository
{
    Task<bool> ExistsByNumberAsync(DocumentNumber number);

    Task AddAsync(ReceiptDocument receiptDocument);
}