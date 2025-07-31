namespace WMS.Application.Clients.Common;

public record ClientResult
(
    Guid Id,
    string Name,
    AddressResult? Address);

public record AddressResult(
    string Street,
    string? City,
    string? PostalCode
);