using ErrorOr;
using MediatR;
using WMS.Application.Clients.Common;

namespace WMS.Application.Clients.Commands.Create;

public record CreateClientCommand(
    string Name,
    string address
) : IRequest<ErrorOr<ClientResult>>;