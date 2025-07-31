using ErrorOr;
using MediatR;
using WMS.Application.Clients.Common;
using WMS.Application.Common.Interface.Persistence;
using WMS.Domain.Common.ErrorCatalog;

namespace WMS.Application.Clients.Queries.GetById;

public class GetByIdClientQueryHandler :
    IRequestHandler<GetByIdClientQuery, ErrorOr<ClientResult>>
{
    private readonly IClientRepository _clientRepository;

    public GetByIdClientQueryHandler(IClientRepository clientRepository)
    {
        _clientRepository = clientRepository;
    }

    public async Task<ErrorOr<ClientResult>> Handle(
        GetByIdClientQuery query,
        CancellationToken cancellationToken)
    {
        var client = await _clientRepository.GetByIdAsync(query.Id);

        if (client is null)
            return Errors.Client.NotFound;

        return ClientResult.From(client);
    }
}