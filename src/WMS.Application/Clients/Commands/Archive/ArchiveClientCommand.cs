using ErrorOr;
using MediatR;
using WMS.Application.Clients.Common;

namespace WMS.Application.Clients.Commands.Archive;

public record ArchiveClientCommand(
    Guid Id
) : IRequest<ErrorOr<ClientResult>>;