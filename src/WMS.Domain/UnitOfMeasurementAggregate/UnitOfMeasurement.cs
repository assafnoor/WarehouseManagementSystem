using ErrorOr;
using WMS.Domain.Common.ErrorCatalog;
using WMS.Domain.Common.Models;
using WMS.Domain.UnitOfMeasurementAggregate.ValueObjects;

namespace WMS.Domain.UnitOfMeasurementAggregate;

public sealed class UnitOfMeasurement : AggregateRoot<UnitOfMeasurementId, Guid>
{
    public string Name { get; private set; }
    public bool IsActive { get; private set; }

    private UnitOfMeasurement(
        UnitOfMeasurementId id,
        string name)
        : base(id)
    {
        Name = name;
        IsActive = true;
    }

    public static UnitOfMeasurement Create(string name)
    {
        return new UnitOfMeasurement(
            UnitOfMeasurementId.CreateUnique(),
            name);
    }

    public void ChangeName(string name)
    {
        Name = name.Trim();
    }

    public ErrorOr<Success> Archive()
    {
        if (!IsActive)
            return Errors.UnitOfMeasurement.AlreadyArchived;

        IsActive = false;
        return Result.Success;
    }

    public ErrorOr<Success> Activate()
    {
        if (IsActive)
            return Errors.UnitOfMeasurement.AlreadyActive;

        IsActive = true;
        return Result.Success;
    }

    public bool IsActivated() => IsActive;

    public bool IsArchived() => !IsActive;

    public ErrorOr<Success> CanBeArchived(bool isUsedInDocuments)
    {
        if (isUsedInDocuments)
            return Errors.UnitOfMeasurement.CannotArchiveInUse;

        return Archive();
    }

#pragma warning disable CS8618

    private UnitOfMeasurement()
    { }

#pragma warning restore CS8618
}