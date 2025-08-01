using Mapster;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using WMS.API.Controllers.Common;
using WMS.Application.Resources.Commands.Activate;
using WMS.Application.Resources.Commands.Archive;
using WMS.Application.Resources.Commands.Create;
using WMS.Application.Resources.Commands.Update;
using WMS.Application.Resources.Queries.GetAll;
using WMS.Application.Resources.Queries.GetById;
using WMS.Contracts.Clients;
using WMS.Contracts.UnitOfMeasurements.Resources;

namespace WMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReceiptDocumentsController : ApiController
    {
        private readonly IMapper _mapper;
        private readonly ISender _mediator;

        public ReceiptDocumentsController(ISender mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        #region Command

        [HttpPost("Add")]
        public async Task<IActionResult> CreateResourcesController(
            AddResourceRequest request
            )
        {
            var command = _mapper.Map<ResourceCommand>((request));

            var result = await _mediator.Send(command);
            return result.Match(
                resource => Ok(_mapper.Map<ResourceResponse>(resource)),
                errors => Problem(errors));
        }

        [HttpPost("Archive")]
        public async Task<IActionResult> ArchiveResource(
        ArchiveResourceRequest request
        )
        {
            var command = _mapper.Map<ArchiveResourceCommand>((request));

            var result = await _mediator.Send(command);
            return result.Match(
                resource => Ok(_mapper.Map<ResourceResponse>(resource)),
                errors => Problem(errors));
        }

        [HttpPost("Activate")]
        public async Task<IActionResult> ActivateResource(
        ActivateResourceRequest request
        )
        {
            var command = _mapper.Map<ActivateResourceCommand>((request));

            var result = await _mediator.Send(command);
            return result.Match(
                resource => Ok(_mapper.Map<ResourceResponse>(resource)),
                errors => Problem(errors));
        }

        [HttpPost("Update")]
        public async Task<IActionResult> UpdateResource(
             UpdateResourceRequest request
            )
        {
            var command = _mapper.Map<UpdateResourceCommand>((request));

            var result = await _mediator.Send(command);
            return result.Match(
                resource => Ok(_mapper.Map<ResourceResponse>(resource)),
                errors => Problem(errors));
        }

        #endregion Command

        #region Query

        [HttpGet("GetById/{Id}")]
        public async Task<IActionResult> GetByIdResource([FromRoute] Guid Id)
        {
            var query = _mapper.Map<GetByIdResourceQuery>(Id);

            var result = await _mediator.Send(query);

            return result.Match(
                Resource => Ok(_mapper.Map<ResourceResponse>(Resource)),
                errors => Problem(errors));
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAllClients([FromQuery] GetAllClientsRequest request)
        {
            var query = _mapper.Map<GetAllResourceQuery>(request);
            var result = await _mediator.Send(query);

            return result.Match(
            Resource => Ok(Resource.Adapt<List<ResourceResponse>>()),
            errors => Problem(errors));
        }

        #endregion Query
    }
}