using WMS.Domain.Common.Models;
using WMS.Domain.ResourceAggregate.ValueObjects;

namespace WMS.Domain.ResourceAggregate;

public sealed class Resource : AggregateRoot<ResourceId, Guid>
{
    public string Name { get; private set; }
    public bool IsActive { get; private set; }

    private Resource(
        ResourceId id,
        string name)
        : base(id)
    {
        Name = name;
        IsActive = true;
    }

    public static Resource Create(string name)
    {
        return new Resource(ResourceId.CreateUnique(), name);
    }
}