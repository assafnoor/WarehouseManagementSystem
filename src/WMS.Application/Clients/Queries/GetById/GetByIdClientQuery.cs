using ErrorOr;
using MediatR;
using WMS.Application.Clients.Common;

namespace WMS.Application.Clients.Queries.GetById;

public record GetByIdClientQuery(
    Guid Id
) : IRequest<ErrorOr<ClientResult>>;
