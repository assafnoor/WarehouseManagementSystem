using WMS.Domain.Common.Models;
using WMS.Domain.ReceiptDocumentAggregate.Entites;
using WMS.Domain.ReceiptDocumentAggregate.ValueObjects;

namespace WMS.Domain.ReceiptDocumentAggregate.Events;

public record ReceiptDocumentDeletedEvent(ReceiptDocumentId ReceiptDocumentId, IReadOnlyList<ReceiptResource> ReceiptResources) : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}