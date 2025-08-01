using ErrorOr;
using MediatR;
using WMS.Application.Common.Interface.Persistence;
using WMS.Application.ReceiptDocuments.Common;
using WMS.Domain.Common.ErrorCatalog;
using WMS.Domain.Common.ValueObjects;
using WMS.Domain.ReceiptDocumentAggregate;
using WMS.Domain.ResourceAggregate.ValueObjects;
using WMS.Domain.UnitOfMeasurementAggregate.ValueObjects;

namespace WMS.Application.ReceiptDocuments.Commands.Create;

public class CreateReceiptDocumentCommandHandler :
    IRequestHandler<CreateReceiptDocumentCommand, ErrorOr<ReceiptDocumentResult>>
{
    private readonly IReceiptDocumentRepository _receiptDocumentRepository;
    private readonly IResourceRepository _resourceRepository;
    private readonly IUnitOfMeasurementRepository _unitOfMeasurementRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateReceiptDocumentCommandHandler(IReceiptDocumentRepository receiptDocumentRepository, IUnitOfWork unitOfWork, IUnitOfMeasurementRepository unitOfMeasurementRepository, IResourceRepository resourceRepository)
    {
        _receiptDocumentRepository = receiptDocumentRepository;
        _unitOfWork = unitOfWork;
        _unitOfMeasurementRepository = unitOfMeasurementRepository;
        _resourceRepository = resourceRepository;
    }

    public async Task<ErrorOr<ReceiptDocumentResult>> Handle(
        CreateReceiptDocumentCommand command,
        CancellationToken cancellationToken)
    {
        var exists = await _receiptDocumentRepository.ExistsByNumberAsync(command.DocumentNumber);
        if (exists)
        {
            return Errors.ReceiptDocument.DuplicateNumber;
        }
        foreach (var resourceCmd in command.ReceiptResources)
        {
            var resourceExists = await _resourceRepository.ExistsActiveAsync(resourceCmd.ResourceId);
            var uomExists = await _unitOfMeasurementRepository.ExistsActiveAsync(resourceCmd.UnitOfMeasurementId);
            if (!resourceExists || !uomExists)
            {
                return Errors.ReceiptDocument.InvalidResource;
            }
        }
        var receiptDocument = ReceiptDocument.Create(DocumentNumber.CreateNew(command.DocumentNumber), command.Date);

        foreach (var resourceCmd in command.ReceiptResources)
        {
            var quantity = Quantity.CreateNew(resourceCmd.Quantity);

            if (quantity.IsError)
                return quantity.Errors;

            receiptDocument.AddResource(
                ResourceId.Create(resourceCmd.ResourceId),
                UnitOfMeasurementId.Create(resourceCmd.UnitOfMeasurementId),
                quantity.Value
                );
        }
        await _receiptDocumentRepository.AddAsync(receiptDocument);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new ReceiptDocumentResult(
            receiptDocument.Id.Value,
            receiptDocument.Number.Value,
            receiptDocument.Date,
            receiptDocument.ReceiptResources.Select(r => new ReceiptResourceResult(
                r.ResourceId.Value,
                r.UnitOfMeasurementId.Value,
                r.Quantity.Value
            )).ToList()
        );
    }
}