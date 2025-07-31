namespace WMS.Contracts.Clients;

public record ClientResponse
(
     Guid Id,
     string Name,
     bool IsActive,
     string address);