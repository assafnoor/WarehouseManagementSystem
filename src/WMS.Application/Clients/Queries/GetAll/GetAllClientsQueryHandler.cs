using ErrorOr;
using MediatR;
using WMS.Application.Clients.Common;
using WMS.Application.Common.Interface.Persistence;
using WMS.Domain.ClientAggregate;

namespace WMS.Application.Clients.Queries.GetAll;

public class GetAllClientsQueryHandler :
    IRequestHandler<GetAllClientsQuery, ErrorOr<IEnumerable<ClientResult>>>
{
    private readonly IClientRepository _clientRepository;

    public GetAllClientsQueryHandler(IClientRepository clientRepository)
    {
        _clientRepository = clientRepository;
    }

    public async Task<ErrorOr<IEnumerable<ClientResult>>> Handle(
        GetAllClientsQuery query,
        CancellationToken cancellationToken)
    {
        var clients = await _clientRepository.GetAllAsync(query.Status, query.Page, query.PageSize)
            ?? Array.Empty<Client>();
        var results = clients.Select(ClientResult.From).ToList();

        return results;
    }
}