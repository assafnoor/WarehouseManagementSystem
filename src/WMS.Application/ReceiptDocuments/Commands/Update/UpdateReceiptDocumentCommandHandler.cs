using ErrorOr;
using MediatR;
using WMS.Application.Common.Interface.Persistence;
using WMS.Application.ReceiptDocuments.Common;
using WMS.Domain.Common.ErrorCatalog;
using WMS.Domain.Common.ValueObjects;
using WMS.Domain.ReceiptDocumentAggregate.ValueObjects;
using WMS.Domain.ResourceAggregate.ValueObjects;
using WMS.Domain.UnitOfMeasurementAggregate.ValueObjects;

namespace WMS.Application.ReceiptDocuments.Commands.Update;

public class UpdateReceiptDocumentCommandHandler :
    IRequestHandler<UpdateReceiptDocumentCommand, ErrorOr<ReceiptDocumentResult>>
{
    private readonly IReceiptDocumentRepository _receiptDocumentRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IResourceRepository _resourceRepository;
    private readonly IUnitOfMeasurementRepository _unitOfMeasurementRepository;

    public UpdateReceiptDocumentCommandHandler(IReceiptDocumentRepository receiptDocumentRepository, IUnitOfWork unitOfWork, IUnitOfMeasurementRepository unitOfMeasurementRepository, IResourceRepository resourceRepository)
    {
        _receiptDocumentRepository = receiptDocumentRepository;
        _unitOfWork = unitOfWork;
        _unitOfMeasurementRepository = unitOfMeasurementRepository;
        _resourceRepository = resourceRepository;
    }

    public async Task<ErrorOr<ReceiptDocumentResult>> Handle(
      UpdateReceiptDocumentCommand command,
      CancellationToken cancellationToken)
    {
        var existingDocument = await _receiptDocumentRepository.GetByIdAsync(ReceiptDocumentId.Create(command.Id));
        if (existingDocument is null)
        {
            return Errors.ReceiptDocument.NotFound;
        }

        var isDuplicateNumber = await _receiptDocumentRepository.ExistsByNumberAsync(
            DocumentNumber.CreateNew(command.DocumentNumber),
            ReceiptDocumentId.Create(command.Id));

        if (isDuplicateNumber)
        {
            return Errors.ReceiptDocument.DuplicateNumber;
        }

        // دمج الموارد المتكررة وتجميع الكميات
        var mergedResources = new List<(Guid ResourceId, Guid UnitOfMeasurementId, decimal TotalQuantity)>();

        if (command.ReceiptResources is not null && command.ReceiptResources.Any())
        {
            mergedResources = command.ReceiptResources
                .GroupBy(r => new { r.ResourceId, r.UnitOfMeasurementId })
                .Select(g => (g.Key.ResourceId, g.Key.UnitOfMeasurementId, g.Sum(x => x.Quantity)))
                .ToList();

            // التحقق من وجود الموارد ووحدات القياس
            foreach (var (resourceId, unitOfMeasurementId, _) in mergedResources)
            {
                var resourceExists = await _resourceRepository.ExistsActiveAsync(ResourceId.Create(resourceId));
                var uomExists = await _unitOfMeasurementRepository.ExistsActiveAsync(UnitOfMeasurementId.Create(unitOfMeasurementId));

                if (!resourceExists || !uomExists)
                {
                    return Errors.ReceiptDocument.InvalidResource;
                }
            }
        }

        // تحديث بيانات المستند
        existingDocument.ChangeDate(command.Date);
        existingDocument.ChangeNumber(DocumentNumber.CreateNew(command.DocumentNumber));

        // إنشاء dictionary للموارد الجديدة باستخدام mergedResources
        var newResources = new Dictionary<(ResourceId, UnitOfMeasurementId), Quantity>();

        foreach (var (resourceId, unitOfMeasurementId, totalQuantity) in mergedResources)
        {
            var resourceIdObj = ResourceId.Create(resourceId);
            var unitIdObj = UnitOfMeasurementId.Create(unitOfMeasurementId);
            var quantityResult = Quantity.CreateNew(totalQuantity);

            if (quantityResult.IsError)
            {
                return quantityResult.FirstError;
            }

            // الآن لن يكون هناك تكرار لأننا استخدمنا mergedResources
            newResources.Add((resourceIdObj, unitIdObj), quantityResult.Value);
        }

        // الحصول على الموارد الموجودة حالياً
        var existingResources = existingDocument.ReceiptResources.ToDictionary(
            r => (r.ResourceId, r.UnitOfMeasurementId),
            r => r.Quantity
        );

        // حذف الموارد التي لم تعد موجودة في الطلب الجديد
        foreach (var (key, _) in existingResources)
        {
            if (!newResources.ContainsKey(key))
            {
                existingDocument.RemoveResource(key.Item1, key.Item2);
            }
        }

        // إضافة أو تحديث الموارد الجديدة
        foreach (var (key, newQty) in newResources)
        {
            var (resourceId, unitId) = key;

            if (existingResources.TryGetValue(key, out var oldQty))
            {
                // تحديث الكمية إذا كانت مختلفة
                if (!oldQty.Equals(newQty))
                {
                    existingDocument.UpdateResourceQuantity(resourceId, unitId, newQty);
                }
            }
            else
            {
                existingDocument.AddResource(resourceId, unitId, newQty);
            }
        }

        _receiptDocumentRepository.Update(existingDocument);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new ReceiptDocumentResult(
            existingDocument.Id.Value,
            existingDocument.Number.Value,
            existingDocument.Date,
            existingDocument.ReceiptResources.Select(r => new ReceiptResourceResult(
                r.ResourceId.Value,
                r.UnitOfMeasurementId.Value,
                r.Quantity.Value
            )).ToList()
        );
    }
}