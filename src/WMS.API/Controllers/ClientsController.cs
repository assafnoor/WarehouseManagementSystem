using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using WMS.API.Controllers.Common;
using WMS.Application.Clients.Commands.Create;
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
}