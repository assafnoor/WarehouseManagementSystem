using WMS.Domain.ClientAggregate.ValueObjects;
using WMS.Domain.Common.Models;
using WMS.Domain.Common.ValueObjects;
using WMS.Domain.ShipmentDocumentAggregate.Entites;
using WMS.Domain.ShipmentDocumentAggregate.ValueObjects;

namespace WMS.Domain.ShipmentDocumentAggregate;

public sealed class ShipmentDocument : AggregateRoot<ShipmentDocumentId, Guid>
{
    public DocumentNumber Number { get; private set; }
    public ClientId ClientId { get; private set; }
    public DateTime Date { get; private set; }
    public ShipmentStatus Status { get; private set; }

    private readonly List<ShipmentResource> _shipmentResources;
    public IReadOnlyCollection<ShipmentResource> ShipmentResources => _shipmentResources.AsReadOnly();
    public DateTime CreatedDateTime { get; private set; }
    public DateTime UpdatedDateTime { get; private set; }

    private ShipmentDocument(ShipmentDocumentId id, DocumentNumber number, ClientId clientId, DateTime date) : base(id)
    {
        Number = number;
        ClientId = clientId;
        Date = date;
        Status = ShipmentStatus.Draft;
        _shipmentResources = new List<ShipmentResource>();
        CreatedDateTime = DateTime.UtcNow;
        UpdatedDateTime = DateTime.UtcNow;
    }

    public static ShipmentDocument Create(DocumentNumber number, ClientId clientId, DateTime date)
    {
        return new ShipmentDocument(ShipmentDocumentId.CreateUnique(), number, clientId, date);
    }
}