using WMS.Domain.ReceiptDocumentAggregate;

namespace WMS.Application.Common.Interface.Persistence;

public interface IReceiptDocumentRepository
{
    Task<bool> ExistsByNumberAsync(string number);

    Task AddAsync(ReceiptDocument receiptDocument);
}