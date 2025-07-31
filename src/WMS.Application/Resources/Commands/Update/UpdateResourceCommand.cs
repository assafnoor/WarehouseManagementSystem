using ErrorOr;
using MediatR;
using WMS.Application.Resources.Common;

namespace WMS.Application.Resources.Commands.Update;

public record UpdateResourceCommand(
    Guid Id,
    string Name
) : IRequest<ErrorOr<ResourceResult>>;