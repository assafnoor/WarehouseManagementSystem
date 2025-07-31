namespace WMS.Domain.ShipmentDocumentAggregate;

//[SuppressMessage("Microsoft.Naming", "CA1720:IdentifiersShouldNotContainTypeNames", MessageId = "Signed")]
public enum ShipmentStatus
{
    Draft = 0,      // Created but not signed
    SignedOff = 1,     // Signed and affects balance
    Cancelled = 2   // Cancelled/withdrawn
}