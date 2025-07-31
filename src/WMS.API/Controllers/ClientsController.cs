using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using WMS.API.Controllers.Common;
using WMS.Application.Clients.Commands.Activate;
using WMS.Application.Clients.Commands.Archive;
using WMS.Application.Clients.Commands.Create;
using WMS.Application.Clients.Commands.Update;
using WMS.Application.Clients.Queries.GetById;
using WMS.Contracts.Clients;

namespace WMS.Api.Controllers;

[Route("api/[controller]")]
public class ClientsController : ApiController
{
    private readonly IMapper _mapper;
    private readonly ISender _mediator;

    public ClientsController(IMapper mapper, ISender mediator)
    {
        _mapper = mapper;
        _mediator = mediator;
    }

    #region Command

    [HttpPost("Add")]
    public async Task<IActionResult> CreateClient(
        AddClientRequest request
        )
    {
        var command = _mapper.Map<CreateClientCommand>((request));

        var createResult = await _mediator.Send(command);
        return createResult.Match(
            client => Ok(_mapper.Map<ClientResponse>(client)),
            errors => Problem(errors));
    }

    [HttpPost("Archive")]
    public async Task<IActionResult> ArchiveClient(
    ArchiveClientRequest request
    )
    {
        var command = _mapper.Map<ArchiveClientCommand>((request));

        var result = await _mediator.Send(command);
        return result.Match(
            client => Ok(_mapper.Map<ClientResponse>(client)),
            errors => Problem(errors));
    }

    [HttpPost("Activate")]
    public async Task<IActionResult> ActivateClient(
    ActivateClientRequest request
    )
    {
        var command = _mapper.Map<ActivateClientCommand>((request));

        var result = await _mediator.Send(command);
        return result.Match(
            client => Ok(_mapper.Map<ClientResponse>(client)),
            errors => Problem(errors));
    }

    [HttpPost("Update")]
    public async Task<IActionResult> UpdateClient(
         UpdateClientRequest request
        )
    {
        var command = _mapper.Map<UpdateClientCommand>((request));

        var result = await _mediator.Send(command);
        return result.Match(
            client => Ok(_mapper.Map<ClientResponse>(client)),
            errors => Problem(errors));
    }

    #endregion Command

    #region Query

    [HttpGet("GetById/{Id}")]
    public async Task<IActionResult> GetByIdClient([FromRoute] Guid Id)
    {
        var query = _mapper.Map<GetByIdClientQuery>(Id);

        var result = await _mediator.Send(query);

        return result.Match(
            client => Ok(_mapper.Map<ClientResponse>(client)),
            errors => Problem(errors));
    }

    #endregion Query
}