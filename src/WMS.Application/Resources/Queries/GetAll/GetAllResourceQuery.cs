using ErrorOr;
using MediatR;
using WMS.Application.Resources.Common;

namespace WMS.Application.Resources.Queries.GetAll;

public record GetAllResourceQuery(
  bool? Status,
  int Page,
  int PageSize) : IRequest<ErrorOr<IEnumerable<ResourceResult>>>;