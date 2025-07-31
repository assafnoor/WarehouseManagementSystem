using ErrorOr;
using MediatR;
using WMS.Application.Resources.Common;

namespace WMS.Application.Resources.Commands.Activate;

public record ActivateResourceCommand(
    Guid Id
) : IRequest<ErrorOr<ResourceResult>>;