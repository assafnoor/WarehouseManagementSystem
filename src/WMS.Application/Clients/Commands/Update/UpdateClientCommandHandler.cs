using ErrorOr;
using MediatR;
using WMS.Application.Clients.Common;
using WMS.Application.Common.Interface.Persistence;
using WMS.Domain.Common.ErrorCatalog;
using WMS.Domain.Common.ValueObjects;

namespace WMS.Application.Clients.Commands.Update;

public class UpdateClientCommandHandler :
    IRequestHandler<UpdateClientCommand, ErrorOr<ClientResult>>
{
    private readonly IClientRepository _clientRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateClientCommandHandler(IClientRepository clientRepository, IUnitOfWork unitOfWork = null)
    {
        _clientRepository = clientRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ErrorOr<ClientResult>> Handle(
        UpdateClientCommand command,
        CancellationToken cancellationToken)
    {
        var client = await _clientRepository.GetByIdAsync(command.Id);
        if (client is null)
            return Errors.Client.NotFound;

        if (client.IsArchived())
            return Errors.Client.Archived;

        // Only check for name duplication if the name is actually changing
        if (!client.Name.Equals(command.Name.Trim(), StringComparison.OrdinalIgnoreCase))
        {
            var existingClient = await _clientRepository.GetByNameAsync(command.Name);
            if (existingClient is not null && existingClient.Id != client.Id)
                return Errors.Client.NameAlreadyExists;
        }

        client.ChangeName(command.Name);
        client.ChangeAddress(Address.CreateNew(command.address, null, null));

        await _clientRepository.UpdateAsync(client);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return ClientResult.From(client);
    }
}