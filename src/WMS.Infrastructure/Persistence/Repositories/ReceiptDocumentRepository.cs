using Microsoft.EntityFrameworkCore;
using WMS.Application.Common.Interface.Persistence;
using WMS.Domain.Common.ValueObjects;
using WMS.Domain.ReceiptDocumentAggregate;

namespace WMS.Infrastructure.Persistence.Repositories;

public class ReceiptDocumentRepository : IReceiptDocumentRepository
{
    private readonly MyAppDbContext _context;

    public ReceiptDocumentRepository(MyAppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(ReceiptDocument receiptDocument)
    {
        await _context.ReceiptDocuments
            .AddAsync(receiptDocument);
    }

    public async Task<bool> ExistsByNumberAsync(DocumentNumber number)
    {
        return await _context.ReceiptDocuments
                .AnyAsync(d => d.Number == number);
    }
}