using ErrorOr;
using WMS.Domain.ClientAggregate.ValueObjects;
using WMS.Domain.Common.ErrorCatalog;
using WMS.Domain.Common.Models;
using WMS.Domain.Common.ValueObjects;

namespace WMS.Domain.ClientAggregate;

public sealed class Client : AggregateRoot<ClientId, Guid>
{
    public string Name { get; private set; }
    public Address Address { get; private set; }
    public bool IsActive { get; private set; }

    private Client(ClientId id, string name, Address address) : base(id)
    {
        Name = name;
        Address = address;
        IsActive = true;
    }

    public static Client Create(string name, Address address)
    {
        return new Client(ClientId.CreateUnique(), name, address);
    }

    public ErrorOr<Updated> Archive()
    {
        if (!IsActive)
            return Errors.Client.AlreadyArchived;

        IsActive = false;
        return Result.Updated;
    }

    public ErrorOr<Updated> Activate()
    {
        if (IsActive)
            return Errors.Client.AlreadyActive;

        IsActive = true;
        return Result.Updated;
    }
}