using ErrorOr;
using WMS.Domain.BalanceAggregate.ValueObjects;
using WMS.Domain.Common.Models;
using WMS.Domain.Common.ValueObjects;
using WMS.Domain.ResourceAggregate.ValueObjects;
using WMS.Domain.UnitOfMeasurementAggregate.ValueObjects;

namespace WMS.Domain.BalanceAggregate;

public sealed class Balance : AggregateRoot<BalanceId, Guid>
{
    public ResourceId ResourceId { get; private set; }
    public UnitOfMeasurementId UnitOfMeasurementId { get; private set; }
    public Quantity Quantity { get; private set; }
    public DateTime CreatedDateTime { get; private set; }
    public DateTime UpdatedDateTime { get; private set; }

    private Balance(BalanceId id, ResourceId resourceId, UnitOfMeasurementId unitOfMeasurementId, Quantity quantity) : base(id)
    {
        ResourceId = resourceId;
        UnitOfMeasurementId = unitOfMeasurementId;
        Quantity = quantity;
        CreatedDateTime = DateTime.UtcNow;
        UpdatedDateTime = DateTime.UtcNow;
    }

    public static ErrorOr<Balance> Create(ResourceId resourceId, UnitOfMeasurementId unitOfMeasurementId, Quantity quantity)
    {
        var balance = new Balance(BalanceId.CreateUnique(), resourceId, unitOfMeasurementId, quantity);
        return balance;
    }

    public ErrorOr<Updated> IncreaseQuantity(Quantity amount)
    {
        var result = Quantity.Add(amount);
        if (result.IsError)
            return result.Errors;

        Quantity = result.Value;
        return Result.Updated;
    }

    public ErrorOr<Updated> DecreaseQuantity(Quantity amount)
    {
        var result = Quantity.Subtract(amount);
        if (result.IsError)
            return result.Errors;

        Quantity = result.Value;
        return Result.Updated;
    }

    public bool HasSufficientQuantity(Quantity requiredQuantity)
    {
        return !Quantity.IsLessThan(requiredQuantity);
    }

#pragma warning disable CS8618

    private Balance()
    { }

#pragma warning restore CS8618
}