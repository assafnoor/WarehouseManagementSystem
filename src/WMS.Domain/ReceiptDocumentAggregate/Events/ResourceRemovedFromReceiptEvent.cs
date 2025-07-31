using WMS.Domain.Common.Models;
using WMS.Domain.Common.ValueObjects;
using WMS.Domain.ReceiptDocumentAggregate.ValueObjects;
using WMS.Domain.ResourceAggregate.ValueObjects;
using WMS.Domain.UnitOfMeasurementAggregate.ValueObjects;

namespace WMS.Domain.ReceiptDocumentAggregate.Events;

public record ResourceRemovedFromReceiptEvent(
 ReceiptDocumentId ReceiptDocumentId,
 ResourceId ResourceId,
 UnitOfMeasurementId UnitOfMeasurementId,
 Quantity Quantity
) : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}