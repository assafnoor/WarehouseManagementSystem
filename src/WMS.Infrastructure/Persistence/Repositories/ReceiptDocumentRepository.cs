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

    public void Remove(ReceiptDocument receiptDocument)
    {
        _context.ReceiptDocuments.Remove(receiptDocument);
    }

    public async Task<IEnumerable<ReceiptDocument>> GetAllAsync(
    DateTime? fromDate,
    DateTime? toDate,
    List<string>? documentNumbers,
    List<Guid>? resourceIds,
    List<Guid>? unitIds,
     int pageNumber = 1,
     int pageSize = 20
        )
    {
        var query = _context.ReceiptDocuments
            .Include(rd => rd.ReceiptResources)
            .AsQueryable();

        if (fromDate.HasValue)
        {
            query = query.Where(rd => rd.Date >= fromDate.Value);
        }

        if (toDate.HasValue)
        {
            query = query.Where(rd => rd.Date <= toDate.Value);
        }

        if (documentNumbers is not null && documentNumbers.Any())
        {
            query = query.Where(rd => documentNumbers.Contains(rd.Number.Value));
        }

        if (resourceIds is not null && resourceIds.Any())
        {
            query = query.Where(rd =>
                rd.ReceiptResources.Any(r => resourceIds.Contains(r.ResourceId.Value)));
        }

        if (unitIds is not null && unitIds.Any())
        {
            query = query.Where(rd =>
                rd.ReceiptResources.Any(r => unitIds.Contains(r.UnitOfMeasurementId.Value)));
        }

        query = query.OrderByDescending(rd => rd.Date)
                   .Skip((pageNumber - 1) * pageSize)
                   .Take(pageSize);
        return await query.ToListAsync();
    }
}