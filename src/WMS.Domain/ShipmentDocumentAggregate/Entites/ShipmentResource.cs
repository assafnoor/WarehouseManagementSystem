using WMS.Domain.Common.Models;
using WMS.Domain.Common.ValueObjects;
using WMS.Domain.ResourceAggregate.ValueObjects;
using WMS.Domain.ShipmentDocumentAggregate.ValueObjects;
using WMS.Domain.UnitOfMeasurementAggregate.ValueObjects;

namespace WMS.Domain.ShipmentDocumentAggregate.Entites;

public sealed class ShipmentResource : Entity<ShipmentResourceId>
{
    public ResourceId ResourceId { get; private set; }
    public UnitOfMeasurementId UnitOfMeasurementId { get; private set; }
    public Quantity Quantity { get; private set; }
    public DateTime CreatedDateTime { get; private set; }
    public DateTime UpdatedDateTime { get; private set; }

    private ShipmentResource(ShipmentResourceId id, ResourceId resourceId, UnitOfMeasurementId unitOfMeasurementId, Quantity quantity) : base(id)
    {
        ResourceId = resourceId;
        UnitOfMeasurementId = unitOfMeasurementId;
        Quantity = quantity;
        CreatedDateTime = DateTime.UtcNow;
        UpdatedDateTime = DateTime.UtcNow;
    }

    internal static ShipmentResource Create(ResourceId resourceId, UnitOfMeasurementId unitOfMeasurementId, Quantity quantity)
    {
        return new ShipmentResource(ShipmentResourceId.CreateUnique(), resourceId, unitOfMeasurementId, quantity);
    }

    private ShipmentResource()
    { }
}