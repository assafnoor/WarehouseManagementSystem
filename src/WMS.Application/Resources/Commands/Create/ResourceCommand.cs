using ErrorOr;
using MediatR;
using WMS.Application.Resources.Common;

namespace WMS.Application.Resources.Commands.Create;

public record ResourceCommand(
    string Name
     ) : IRequest<ErrorOr<ResourceResult>>;