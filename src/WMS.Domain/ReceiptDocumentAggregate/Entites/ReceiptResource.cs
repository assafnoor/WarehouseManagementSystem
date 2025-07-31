using WMS.Domain.Common.Models;
using WMS.Domain.Common.ValueObjects;
using WMS.Domain.ReceiptDocumentAggregate.ValueObjects;
using WMS.Domain.ResourceAggregate.ValueObjects;
using WMS.Domain.UnitOfMeasurementAggregate.ValueObjects;

namespace WMS.Domain.ReceiptDocumentAggregate.Entites;

public sealed class ReceiptResource : Entity<ReceiptResourceId>
{
    public ResourceId ResourceId { get; private set; }
    public UnitOfMeasurementId UnitOfMeasurementId { get; private set; }
    public Quantity Quantity { get; private set; }
    public DateTime CreatedDateTime { get; private set; }
    public DateTime UpdatedDateTime { get; private set; }

    private ReceiptResource(ReceiptResourceId id, ResourceId resourceId, UnitOfMeasurementId unitOfMeasurementId, Quantity quantity) : base(id)
    {
        ResourceId = resourceId;
        UnitOfMeasurementId = unitOfMeasurementId;
        Quantity = quantity;
        CreatedDateTime = DateTime.UtcNow;
        UpdatedDateTime = DateTime.UtcNow;
    }

    public static ReceiptResource Create(ResourceId resourceId, UnitOfMeasurementId unitOfMeasurementId, Quantity quantity)
    {
        return new ReceiptResource(ReceiptResourceId.CreateUnique(), resourceId, unitOfMeasurementId, quantity);
    }

    public void ChangeQuantity(Quantity quantity)
    {
        Quantity = quantity;
    }

#pragma warning disable CS8618

    private ReceiptResource()
    { }

#pragma warning restore CS8618
}