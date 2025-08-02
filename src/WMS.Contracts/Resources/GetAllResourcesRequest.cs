namespace WMS.Contracts.Resources;

public record GetAllResourcesRequest(

   bool? Status,
  int Page = 1,
  int PageSize = 10

);