using ErrorOr;
using MediatR;
using WMS.Application.Resources.Common;

namespace WMS.Application.Resources.Commands.Delete;

public record DeleteResourceCommand(
    Guid Id
) : IRequest<ErrorOr<ResourceResult>>;