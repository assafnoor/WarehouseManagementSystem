using Microsoft.EntityFrameworkCore;
using WMS.Application.Common.Interface.Persistence;
using WMS.Domain.Common.ValueObjects;
using WMS.Domain.ReceiptDocumentAggregate;
using WMS.Domain.ReceiptDocumentAggregate.ValueObjects;

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

    public async Task<ReceiptDocument?> GetByIdAsync(ReceiptDocumentId receiptDocumentId)
    {
        return await _context.ReceiptDocuments
            .FirstOrDefaultAsync(c => c.Id == receiptDocumentId);
    }

    public async Task<bool> ExistsByNumberAsync(DocumentNumber documentNumber, ReceiptDocumentId excludeId)
    {
        return await _context.ReceiptDocuments
                     .Where(rd => rd.Id != excludeId)
                     .AnyAsync(rd => rd.Number == documentNumber);
    }

    public void Update(ReceiptDocument receiptDocument)
    {
        _context.ReceiptDocuments.Update(receiptDocument);
    }
}