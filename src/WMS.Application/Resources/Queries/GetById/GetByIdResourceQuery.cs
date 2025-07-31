using ErrorOr;
using MediatR;
using WMS.Application.Resources.Common;

namespace WMS.Application.Resources.Queries.GetById;

public record GetByIdResourceQuery(
    Guid Id
) : IRequest<ErrorOr<ResourceResult>>;
