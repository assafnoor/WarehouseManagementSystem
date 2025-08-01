using ErrorOr;
using MediatR;
using WMS.Application.Clients.Common;
using WMS.Application.Common.Interface.Persistence;
using WMS.Domain.ClientAggregate.ValueObjects;
using WMS.Domain.Common.ErrorCatalog;

namespace WMS.Application.Clients.Commands.Activate;

public class ActivateClientCommandHandler :
    IRequestHandler<ActivateClientCommand, ErrorOr<ClientResult>>
{
    private readonly IClientRepository _clientRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ActivateClientCommandHandler(IClientRepository clientRepository, IUnitOfWork unitOfWork)
    {
        _clientRepository = clientRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ErrorOr<ClientResult>> Handle(
    ActivateClientCommand command,
        CancellationToken cancellationToken)
    {
        var client = await _clientRepository.GetByIdAsync(ClientId.Create(command.Id));
        if (client is null)
            return Errors.Client.NotFound;

        var activateResult = client.Activate();
        if (activateResult.IsError)
            return activateResult.Errors;

        await _clientRepository.UpdateAsync(client);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return ClientResult.From(client);
    }
}