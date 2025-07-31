using ErrorOr;
using WMS.Domain.Common.ErrorCatalog;
using WMS.Domain.Common.Models;
using WMS.Domain.ResourceAggregate.ValueObjects;

namespace WMS.Domain.ResourceAggregate;

public sealed class Resource : AggregateRoot<ResourceId, Guid>
{
    public string Name { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime CreatedDateTime { get; private set; }
    public DateTime UpdatedDateTime { get; private set; }

    private Resource(
        ResourceId id,
        string name)
        : base(id)
    {
        Name = name;
        IsActive = true;
        CreatedDateTime = DateTime.UtcNow;
        UpdatedDateTime = DateTime.UtcNow;
    }

    public static Resource Create(string name)
    {
        return new Resource(ResourceId.CreateUnique(), name.Trim());
    }

    public void ChangeName(string name)
    {
        Name = name.Trim();
        Touch();
    }

    public ErrorOr<Success> Archive()
    {
        if (!IsActive)
            return Errors.Resource.AlreadyArchived;

        IsActive = false;
        Touch();
        return Result.Success;
    }

    public ErrorOr<Success> Activate()
    {
        if (IsActive)
            return Errors.Resource.AlreadyActive;

        IsActive = true;
        Touch();
        return Result.Success;
    }

    public bool IsActivated() => IsActive;

    public bool IsArchived() => !IsActive;

    public ErrorOr<Success> CanBeArchived(bool isUsedInDocuments)
    {
        if (isUsedInDocuments)
            return Errors.Resource.CannotArchiveInUse;

        return Archive();
    }

    private void Touch()
    {
        UpdatedDateTime = DateTime.UtcNow;
    }

#pragma warning disable CS8618

    private Resource()
    { }

#pragma warning restore CS8618
}