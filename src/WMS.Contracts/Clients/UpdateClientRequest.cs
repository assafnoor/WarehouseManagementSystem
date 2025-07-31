namespace WMS.Contracts.Clients;

public record UpdateClientRequest
(
  Guid Id,
  string Name,
  string Address
  );