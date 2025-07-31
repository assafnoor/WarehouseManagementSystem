using ErrorOr;
using MediatR;
using WMS.Application.Resources.Common;

namespace WMS.Application.Resources.Commands.Archive;

public record ArchiveResourceCommand(
    Guid Id
) : IRequest<ErrorOr<ResourceResult>>;