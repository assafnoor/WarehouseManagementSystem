using MediatR;
using Microsoft.Extensions.Logging;
using WMS.Application.Common.Interface.Persistence;
using WMS.Domain.BalanceAggregate;
using WMS.Domain.ReceiptDocumentAggregate.Events;

namespace WMS.Application.ReceiptDocuments.Events;

public class ResourceAddedToReceiptEventHandler : INotificationHandler<ResourceAddedToReceiptEvent>
{
    private readonly IBalanceRepository _balanceRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<ResourceAddedToReceiptEventHandler> _logger;

    public ResourceAddedToReceiptEventHandler(
        IBalanceRepository balanceRepository,
        IUnitOfWork unitOfWork,
        ILogger<ResourceAddedToReceiptEventHandler> logger)
    {
        _balanceRepository = balanceRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task Handle(ResourceAddedToReceiptEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Handling ResourceAddedToReceiptEvent for Receipt {ReceiptId}, Resource {ResourceId}, UoM {UnitOfMeasurementId}, Quantity {Quantity}",
            notification.ReceiptDocumentId,
            notification.ResourceId,
            notification.UnitOfMeasurementId,
            notification.Quantity);

        try
        {
            // Find existing balance or create new one
            var existingBalance = await _balanceRepository.GetByResourceAndUnitAsync(
                notification.ResourceId,
                notification.UnitOfMeasurementId,
                cancellationToken);

            if (existingBalance is not null)
            {
                // Add to existing balance
                existingBalance.IncreaseQuantity(notification.Quantity);
                _balanceRepository.Update(existingBalance);
            }
            else
            {
                // Create new balance
                var newBalance = Balance.Create(
                    notification.ResourceId,
                    notification.UnitOfMeasurementId,
                    notification.Quantity);

                await _balanceRepository.AddAsync(newBalance.Value, cancellationToken);
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
                "Error updating balance for ResourceAddedToReceiptEvent. Receipt {ReceiptId}, Resource {ResourceId}",
                notification.ReceiptDocumentId,
                notification.ResourceId);
            throw;
        }
    }
}