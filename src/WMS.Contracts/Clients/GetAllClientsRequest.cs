namespace WMS.Contracts.Clients;

public record GetAllClientsRequest(

    bool? Status,
   int Page = 1,
   int PageSize = 10

);