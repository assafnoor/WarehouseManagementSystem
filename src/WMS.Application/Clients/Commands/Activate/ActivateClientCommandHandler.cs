using ErrorOr;
using MediatR;
using WMS.Application.Clients.Common;
using WMS.Application.Common.Interface.Persistence;
using WMS.Domain.Common.ErrorCatalog;

namespace WMS.Application.Clients.Commands.Activate;

public class ActivateClientCommandHandler :
    IRequestHandler<ActivateClientCommand, ErrorOr<ClientResult>>
{
    private readonly IClientRepository _clientRepository;

    public ActivateClientCommandHandler(IClientRepository clientRepository)
    {
        _clientRepository = clientRepository;
    }

    public async Task<ErrorOr<ClientResult>> Handle(
    ActivateClientCommand command,
        CancellationToken cancellationToken)
    {
        var client = await _clientRepository.GetByIdAsync(command.Id);
        if (client is null)
            return Errors.Client.NotFound;

        var activateResult = client.Activate();
        if (activateResult.IsError)
            return activateResult.Errors;

        await _clientRepository.UpdateAsync(client);

        return new ClientResult(client.Id.Value, client.Name,
            new AddressResult(
                client.Address.Street ?? string.Empty,
                client.Address.City,
                client.Address.PostalCode));
    }
}