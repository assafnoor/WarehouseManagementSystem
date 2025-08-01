using ErrorOr;
using WMS.Domain.Common.ErrorCatalog;
using WMS.Domain.Common.Models;
using WMS.Domain.Common.ValueObjects;
using WMS.Domain.ReceiptDocumentAggregate.Entites;
using WMS.Domain.ReceiptDocumentAggregate.Events;
using WMS.Domain.ReceiptDocumentAggregate.ValueObjects;
using WMS.Domain.ResourceAggregate.ValueObjects;
using WMS.Domain.UnitOfMeasurementAggregate.ValueObjects;

namespace WMS.Domain.ReceiptDocumentAggregate;

public sealed class ReceiptDocument : AggregateRoot<ReceiptDocumentId, Guid>
{
    public DocumentNumber Number { get; private set; }
    public DateTime Date { get; private set; }

    private readonly List<ReceiptResource> _receiptResources;
    public IReadOnlyCollection<ReceiptResource> ReceiptResources => _receiptResources.AsReadOnly();
    public DateTime CreatedDateTime { get; private set; }
    public DateTime UpdatedDateTime { get; private set; }

    private ReceiptDocument(ReceiptDocumentId id, DocumentNumber number, DateTime date) : base(id)
    {
        Number = number;
        Date = date;
        _receiptResources = new List<ReceiptResource>();
        CreatedDateTime = DateTime.UtcNow;
        UpdatedDateTime = DateTime.UtcNow;
    }

    public static ReceiptDocument Create(DocumentNumber number, DateTime date)
    {
        return new ReceiptDocument(ReceiptDocumentId.CreateUnique(), number, date);
    }

    public ReceiptResource AddResource(ResourceId resourceId, UnitOfMeasurementId unitOfMeasurementId, Quantity quantity)
    {
        var existingResource = _receiptResources
            .FirstOrDefault(r => r.ResourceId.Equals(resourceId) && r.UnitOfMeasurementId.Equals(unitOfMeasurementId));

        if (existingResource is not null)
        {
            var oldQuantity = existingResource.Quantity;
            existingResource.ChangeQuantity(quantity);
            AddDomainEvent(new ResourceQuantityChangedInReceiptEvent(Id, resourceId, unitOfMeasurementId, oldQuantity, quantity));
            return existingResource;
        }

        var receiptResource = ReceiptResource.Create(resourceId, unitOfMeasurementId, quantity);
        _receiptResources.Add(receiptResource);

        AddDomainEvent(new ResourceAddedToReceiptEvent(Id, resourceId, unitOfMeasurementId, quantity));
        Touch();
        return receiptResource;
    }

    public ErrorOr<Success> RemoveResource(ResourceId resourceId, UnitOfMeasurementId unitOfMeasurementId)
    {
        var resource = _receiptResources
            .FirstOrDefault(r => r.ResourceId.Equals(resourceId) && r.UnitOfMeasurementId.Equals(unitOfMeasurementId));

        if (resource is null)
            return Errors.Doamin.ResourceNotFound;

        var oldQuantity = resource.Quantity;
        _receiptResources.Remove(resource);

        AddDomainEvent(new ResourceRemovedFromReceiptEvent(Id, resourceId, unitOfMeasurementId, oldQuantity));
        Touch();

        return Result.Success;
    }

    public ErrorOr<Success> UpdateResourceQuantity(ResourceId resourceId, UnitOfMeasurementId unitOfMeasurementId, Quantity newQuantity)
    {
        var resource = _receiptResources
            .FirstOrDefault(r => r.ResourceId.Equals(resourceId) && r.UnitOfMeasurementId.Equals(unitOfMeasurementId));

        if (resource is null)
            return Errors.Doamin.ResourceNotFound;

        var oldQuantity = resource.Quantity;
        resource.ChangeQuantity(newQuantity);

        AddDomainEvent(new ResourceQuantityChangedInReceiptEvent(Id, resourceId, unitOfMeasurementId, oldQuantity, newQuantity));
        Touch();
        return Result.Success;
    }

    public void MarkAsDeleted()
    {
        AddDomainEvent(new ReceiptDocumentDeletedEvent(Id, ReceiptResources.ToList()));
        Touch();
    }

    public void ChangeNumber(DocumentNumber newNumber)
    {
        Number = newNumber;
        Touch();
    }

    public void ChangeDate(DateTime newDate)
    {
        Date = newDate;
        Touch();
    }

    public void ClearResources()
    {
        _receiptResources.Clear();
        Touch();
    }

    private void Touch()
    {
        UpdatedDateTime = DateTime.UtcNow;
    }

#pragma warning disable CS8618

    private ReceiptDocument()
    { }

#pragma warning restore CS8618
}