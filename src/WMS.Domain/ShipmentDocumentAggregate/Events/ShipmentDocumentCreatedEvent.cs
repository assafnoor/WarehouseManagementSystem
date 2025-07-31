using WMS.Domain.Common.Models;
using WMS.Domain.ShipmentDocumentAggregate.ValueObjects;

namespace WMS.Domain.ShipmentDocumentAggregate.Events;

public record ShipmentDocumentCreatedEvent(ShipmentDocumentId ShipmentDocumentId) : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}