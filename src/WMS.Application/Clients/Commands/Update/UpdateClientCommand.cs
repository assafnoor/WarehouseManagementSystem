using ErrorOr;
using MediatR;
using WMS.Application.Clients.Common;

namespace WMS.Application.Clients.Commands.Update;

public record UpdateClientCommand(
    Guid Id,
    string Name,
       string address
) : IRequest<ErrorOr<ClientResult>>;