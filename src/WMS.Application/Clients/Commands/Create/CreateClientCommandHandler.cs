using ErrorOr;
using MediatR;
using WMS.Application.Clients.Common;
using WMS.Application.Common.Interface.Persistence;
using WMS.Domain.ClientAggregate;
using WMS.Domain.Common.ErrorCatalog;
using WMS.Domain.Common.ValueObjects;

namespace WMS.Application.Clients.Commands.Create;

public class CreateClientCommandHandler :
    IRequestHandler<CreateClientCommand, ErrorOr<ClientResult>>
{
    private readonly IClientRepository _clientRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateClientCommandHandler(IClientRepository clientRepository, IUnitOfWork unitOfWork)
    {
        _clientRepository = clientRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ErrorOr<ClientResult>> Handle(
        CreateClientCommand command,
        CancellationToken cancellationToken)
    {
        // Check if any client with this name exists (active or archived)
        var existingClient = await _clientRepository.GetByNameAsync(command.Name);
        if (existingClient is not null)
        {
            return Errors.Client.NameAlreadyExists;
        }

        var client = Client.Create(command.Name,
        Address.CreateNew(command.address, null, null));

        await _clientRepository.AddAsync(client);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return ClientResult.From(client);
    }
}