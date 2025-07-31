using ErrorOr;
using MediatR;
using WMS.Application.Clients.Common;

namespace WMS.Application.Clients.Commands.Activate;

public record ActivateClientCommand(
    Guid Id
) : IRequest<ErrorOr<ClientResult>>;