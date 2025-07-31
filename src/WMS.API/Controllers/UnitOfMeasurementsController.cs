using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using WMS.API.Controllers.Common;
using WMS.Application.UnitOfMeasurements.Commands.Activate;
using WMS.Application.UnitOfMeasurements.Commands.Archive;
using WMS.Application.UnitOfMeasurements.Commands.Create;
using WMS.Application.UnitOfMeasurements.Commands.Update;
using WMS.Contracts.UnitOfMeasurements;

namespace WMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UnitOfMeasurementsController : ApiController
    {
        private readonly IMapper _mapper;
        private readonly ISender _mediator;

        public UnitOfMeasurementsController(ISender mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        #region Command

        [HttpPost("Add")]
        public async Task<IActionResult> CreateUnitOfMeasurement(
            AddUnitOfMeasurementRequest request
            )
        {
            var command = _mapper.Map<CreateUnitOfMeasurementCommand>((request));

            var result = await _mediator.Send(command);
            return result.Match(
                unitOfMeasurement => Ok(_mapper.Map<UnitOfMeasurementResponse>(unitOfMeasurement)),
                errors => Problem(errors));
        }

        [HttpPost("Archive")]
        public async Task<IActionResult> ArchiveUnitOfMeasurement(
        ArchiveUnitOfMeasurementRequest request
        )
        {
            var command = _mapper.Map<ArchiveUnitOfMeasurementCommand>((request));

            var result = await _mediator.Send(command);
            return result.Match(
                unitOfMeasurement => Ok(_mapper.Map<UnitOfMeasurementResponse>(unitOfMeasurement)),
                errors => Problem(errors));
        }

        [HttpPost("Activate")]
        public async Task<IActionResult> ActivateUnitOfMeasurement(
        ActivateUnitOfMeasurementRequest request
        )
        {
            var command = _mapper.Map<ActivateUnitOfMeasurementCommand>((request));

            var result = await _mediator.Send(command);
            return result.Match(
                UnitOfMeasurement => Ok(_mapper.Map<UnitOfMeasurementResponse>(UnitOfMeasurement)),
                errors => Problem(errors));
        }

        [HttpPost("Update")]
        public async Task<IActionResult> UpdateUnitOfMeasurement(
             UpdateUnitOfMeasurementRequest request
            )
        {
            var command = _mapper.Map<UpdateUnitOfMeasurementCommand>((request));

            var result = await _mediator.Send(command);
            return result.Match(
                UnitOfMeasurement => Ok(_mapper.Map<UnitOfMeasurementResponse>(UnitOfMeasurement)),
                errors => Problem(errors));
        }

        #endregion Command
    }
}