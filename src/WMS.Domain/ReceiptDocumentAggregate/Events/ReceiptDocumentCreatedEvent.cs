using WMS.Domain.Common.Models;
using WMS.Domain.ReceiptDocumentAggregate.ValueObjects;

namespace WMS.Domain.ReceiptDocumentAggregate.Events;

public record ReceiptDocumentCreatedEvent(ReceiptDocumentId ReceiptDocumentId) : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}