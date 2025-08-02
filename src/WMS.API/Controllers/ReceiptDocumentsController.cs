using Mapster;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using WMS.API.Controllers.Common;
using WMS.Application.ReceiptDocuments.Commands.Create;
using WMS.Application.ReceiptDocuments.Commands.Update;
using WMS.Application.ReceiptDocuments.Queries.GetAll;
using WMS.Application.ReceiptDocuments.Queries.GetById;
using WMS.Contracts.Clients;
using WMS.Contracts.ReceiptDocument;

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
        public async Task<IActionResult> CreateReceiptDocument(
            AddReceiptDocumentRequest request
            )
        {
            var command = _mapper.Map<CreateReceiptDocumentCommand>((request));

            var result = await _mediator.Send(command);
            return result.Match(
                receipt => Ok(receipt.Adapt<ReceiptDocumentResponse>()),
                errors => Problem(errors));
        }

        [HttpPost("Update")]
        public async Task<IActionResult> UpdateReceiptDocument(
             UpdateReceiptDocumentRequest request
            )
        {
            var command = _mapper.Map<UpdateReceiptDocumentCommand>((request));

            var result = await _mediator.Send(command);
            return result.Match(
                resource => Ok(_mapper.Map<ReceiptDocumentResponse>(resource)),
                errors => Problem(errors));
        }

        #endregion Command

        #region Query

        [HttpGet("GetById/{Id}")]
        public async Task<IActionResult> GetByIdReceiptDocument([FromRoute] Guid Id)
        {
            var query = _mapper.Map<GetByIdReceiptDocumentQuery>(Id);

            var result = await _mediator.Send(query);

            return result.Match(
                Resource => Ok(_mapper.Map<ReceiptDocumentResponse>(Resource)),
                errors => Problem(errors));
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAllReceiptDocuments([FromQuery] GetAllClientsRequest request)
        {
            var query = _mapper.Map<GetAllReceiptDocumentQuery>(request);
            var result = await _mediator.Send(query);

            return result.Match(
            Resource => Ok(Resource.Adapt<List<ReceiptDocumentResponse>>()),
            errors => Problem(errors));
        }

        #endregion Query
    }
}