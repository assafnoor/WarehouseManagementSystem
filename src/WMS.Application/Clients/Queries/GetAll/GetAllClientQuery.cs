using ErrorOr;
using MediatR;
using WMS.Application.Clients.Common;

namespace WMS.Application.Clients.Queries.GetAll;

public record GetAllClientQuery(
    Guid Id
) : IRequest<ErrorOr<ClientResult>>;
