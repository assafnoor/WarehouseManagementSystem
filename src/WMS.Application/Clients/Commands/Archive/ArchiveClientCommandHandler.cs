using ErrorOr;
using MediatR;
using WMS.Application.Clients.Common;
using WMS.Application.Common.Interface.Persistence;
using WMS.Domain.Common.ErrorCatalog;

namespace WMS.Application.Clients.Commands.Archive;

public class ArchiveClientCommandHandler :
    IRequestHandler<ArchiveClientCommand, ErrorOr<ClientResult>>
{
    private readonly IClientRepository _clientRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ArchiveClientCommandHandler(IClientRepository clientRepository, IUnitOfWork unitOfWork)
    {
        _clientRepository = clientRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ErrorOr<ClientResult>> Handle(
    ArchiveClientCommand command,
        CancellationToken cancellationToken)
    {
        var client = await _clientRepository.GetByIdAsync(command.Id);
        if (client is null)
            return Errors.Client.NotFound;

        //var canBeArchived = client.CanBeArchived(false);// return
        //if (canBeArchived.IsError)
        //    return canBeArchived.Errors;

        if (client.IsArchived())
            return Errors.Client.Archived;

        var activateResult = client.Archive();
        if (activateResult.IsError)
            return activateResult.Errors;

        await _clientRepository.UpdateAsync(client);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return ClientResult.From(client);
    }
}