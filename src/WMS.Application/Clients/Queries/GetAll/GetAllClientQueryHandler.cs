using ErrorOr;
using MediatR;
using MyApp.Application.Common.Interfaces.Persistance;
using WMS.Application.Clients.Common;
using WMS.Domain.Common.ErrorCatalog;
using WMS.Domain.ClientAggregate;

namespace WMS.Application.Clients.Queries.GetAll;

public class GetAllClientQueryHandler :
    IRequestHandler<GetAllClientQuery, ErrorOr<ClientResult>>
{
    private readonly IClientRepository _lientRepository;

    public GetAllClientQueryHandler(IClientRepository lientRepository)
    {
        _lientRepository = lientRepository;
    }

    public async Task<ErrorOr<ClientResult>> Handle(
        GetAllClientQuery query,
        CancellationToken cancellationToken)
    {
        var lient = await _lientRepository.GetByIdAsync(query.Id);

        if (lient is null)
            return Errors.Client.NotFound;

        return new ClientResult(lient.Id.Value, lient.Name);
    }
}
