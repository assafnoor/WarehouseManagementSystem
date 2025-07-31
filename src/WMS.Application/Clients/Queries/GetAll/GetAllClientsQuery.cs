using ErrorOr;
using MediatR;
using WMS.Application.Clients.Common;

namespace WMS.Application.Clients.Queries.GetAll;
public record GetAllClientsQuery(
    bool? Status,
    int Page,
    int PageSize
) : IRequest<ErrorOr<IEnumerable<ClientResult>>>;