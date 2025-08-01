using MediatR;
using Microsoft.Extensions.Logging;
using WMS.Application.Common.Interface.Persistence;
using WMS.Domain.ReceiptDocumentAggregate.Events;

namespace WMS.Application.ReceiptDocuments.Events;

public class ResourceRemovedFromReceiptEventHandler : INotificationHandler<ResourceRemovedFromReceiptEvent>
{
    private readonly IBalanceRepository _balanceRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<ResourceRemovedFromReceiptEventHandler> _logger;

    public ResourceRemovedFromReceiptEventHandler(
        IBalanceRepository balanceRepository,
        IUnitOfWork unitOfWork,
        ILogger<ResourceRemovedFromReceiptEventHandler> logger)
    {
        _balanceRepository = balanceRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task Handle(ResourceRemovedFromReceiptEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Handling ResourceRemovedFromReceiptEvent for Receipt {ReceiptId}, Resource {ResourceId}, Removed Quantity {RemovedQuantity}",
            notification.ReceiptDocumentId,
            notification.ResourceId,
            notification.Quantity);

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

            // Subtract the removed quantity from balance
            var result = balance.DecreaseQuantity(notification.Quantity);
            if (result.IsError)
            {
                _logger.LogError(
                    "Failed to subtract quantity from balance: {Error}",
                    result.FirstError.Description);
                throw new InvalidOperationException($"Failed to update balance: {result.FirstError.Description}");
            }

            // If balance becomes zero, optionally remove it
            if (balance.Quantity.Value == 0)
            {
                _balanceRepository.Remove(balance);
                _logger.LogInformation(
                    "Removed zero balance for Resource {ResourceId}, UoM {UnitOfMeasurementId}",
                    notification.ResourceId,
                    notification.UnitOfMeasurementId);
            }
            else
            {
                _balanceRepository.Update(balance);
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation(
                "Successfully updated balance for Resource {ResourceId}, UoM {UnitOfMeasurementId}",
                notification.ResourceId,
                notification.UnitOfMeasurementId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "Error updating balance for ResourceRemovedFromReceiptEvent. Receipt {ReceiptId}, Resource {ResourceId}",
                notification.ReceiptDocumentId,
                notification.ResourceId);
            throw;
        }
    }
}