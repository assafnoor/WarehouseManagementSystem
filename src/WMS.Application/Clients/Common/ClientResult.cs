namespace WMS.Application.Clients.Common;

using WMS.Domain.ClientAggregate;

public record ClientResult(
    Guid Id,
    string Name,
    bool IsActive,
    AddressResult? Address)
{
    public static ClientResult From(Client client)
    {
        return new ClientResult(
            client.Id.Value,
            client.Name,
            client.IsActive,
            client.Address is null
                ? null
                : new AddressResult(
                    client.Address.Street ?? string.Empty,
                    client.Address.City,
                    client.Address.PostalCode)
        );
    }
}

public record AddressResult(
    string Street,
    string? City,
    string? PostalCode
);