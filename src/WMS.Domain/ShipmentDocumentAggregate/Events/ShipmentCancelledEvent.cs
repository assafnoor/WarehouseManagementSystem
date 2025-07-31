using WMS.Domain.Common.Models;
using WMS.Domain.ShipmentDocumentAggregate.Entites;
using WMS.Domain.ShipmentDocumentAggregate.ValueObjects;

namespace WMS.Domain.ShipmentDocumentAggregate.Events;

public record ShipmentCancelledEvent(
    ShipmentDocumentId ShipmentDocumentId,
    IReadOnlyList<ShipmentResource> ShipmentResources
) : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}