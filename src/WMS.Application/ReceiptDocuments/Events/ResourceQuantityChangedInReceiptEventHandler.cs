using MediatR;
using Microsoft.Extensions.Logging;
using WMS.Application.Common.Interface.Persistence;
using WMS.Domain.Common.ValueObjects;
using WMS.Domain.ReceiptDocumentAggregate.Events;

namespace WMS.Application.ReceiptDocuments.Events;

public class ResourceQuantityChangedInReceiptEventHandler : INotificationHandler<ResourceQuantityChangedInReceiptEvent>
{
    private readonly IBalanceRepository _balanceRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<ResourceQuantityChangedInReceiptEventHandler> _logger;

    public ResourceQuantityChangedInReceiptEventHandler(
        IBalanceRepository balanceRepository,
        IUnitOfWork unitOfWork,
        ILogger<ResourceQuantityChangedInReceiptEventHandler> logger)
    {
        _balanceRepository = balanceRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task Handle(ResourceQuantityChangedInReceiptEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Handling ResourceQuantityChangedInReceiptEvent for Receipt {ReceiptId}, Resource {ResourceId}, Old Quantity {OldQuantity}, New Quantity {NewQuantity}",
            notification.ReceiptDocumentId,
            notification.ResourceId,
            notification.OldQuantity,
            notification.NewQuantity);

        try
        {
            var balance = await _balanceRepository.GetByResourceAndUnitAsync(
                notification.ResourceId,
                notification.UnitOfMeasurementId,
                cancellationToken);

            if (balance is null)
            {
                _logger.LogWarning(
                    "Balance not found for Resource {ResourceId}, UoM {UnitOfMeasurementId}",
                    notification.ResourceId,
                    notification.UnitOfMeasurementId);
                return;
            }

            // Calculate the difference and apply it
            var quantityDifference = notification.NewQuantity.Value - notification.OldQuantity.Value;

            if (quantityDifference > 0)
            {
                balance.IncreaseQuantity(Quantity.CreateNew(quantityDifference).Value);
            }
            else if (quantityDifference < 0)
            {
                var result = balance.DecreaseQuantity(Quantity.CreateNew(Math.Abs(quantityDifference)).Value);
                if (result.IsError)
                {
                    _logger.LogError(
                        "Failed to subtract quantity from balance: {Error}",
                        result.FirstError.Description);
                    throw new InvalidOperationException($"Failed to update balance: {result.FirstError.Description}");
                }
            }

            _balanceRepository.Update(balance);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation(
                "Successfully updated balance for Resource {ResourceId}, UoM {UnitOfMeasurementId}",
                notification.ResourceId,
                notification.UnitOfMeasurementId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "Error updating balance for ResourceQuantityChangedInReceiptEvent. Receipt {ReceiptId}, Resource {ResourceId}",
                notification.ReceiptDocumentId,
                notification.ResourceId);
            throw;
        }
    }
}