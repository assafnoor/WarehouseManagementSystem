using MediatR;
using Microsoft.Extensions.Logging;
using WMS.Application.Common.Interface.Persistence;
using WMS.Domain.ReceiptDocumentAggregate.Events;

namespace WMS.Application.ReceiptDocuments.Events;

public class ReceiptDocumentDeletedEventHandler : INotificationHandler<ReceiptDocumentDeletedEvent>
{
    private readonly IBalanceRepository _balanceRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<ReceiptDocumentDeletedEventHandler> _logger;

    public ReceiptDocumentDeletedEventHandler(
        IBalanceRepository balanceRepository,
        IUnitOfWork unitOfWork,
        ILogger<ReceiptDocumentDeletedEventHandler> logger)
    {
        _balanceRepository = balanceRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task Handle(ReceiptDocumentDeletedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Handling ReceiptDocumentDeletedEvent for Receipt {ReceiptId}",
            notification.ReceiptDocumentId);

        try
        {
            foreach (var receiptResource in notification.ReceiptResources)
            {
                var balance = await _balanceRepository.GetByResourceAndUnitAsync(
                    receiptResource.ResourceId,
                    receiptResource.UnitOfMeasurementId,
                    cancellationToken);

                if (balance is null)
                {
                    _logger.LogWarning(
                        "Balance not found for Resource {ResourceId}, UoM {UnitOfMeasurementId}",
                        receiptResource.ResourceId,
                        receiptResource.UnitOfMeasurementId);
                    continue;
                }

                // ⚠️ Decrease quantity because we are deleting a receipt
                var result = balance.DecreaseQuantity(receiptResource.Quantity);
                if (result.IsError)
                {
                    _logger.LogError(
                        "Cannot delete receipt: insufficient balance for Resource {ResourceId}, UoM {UnitOfMeasurementId}. Error: {Error}",
                        receiptResource.ResourceId,
                        receiptResource.UnitOfMeasurementId,
                        result.FirstError.Description);

                    throw new InvalidOperationException($"Cannot delete receipt: {result.FirstError.Description}");
                }

                if (balance.Quantity.Value == 0)
                {
                    _balanceRepository.Remove(balance);
                    _logger.LogInformation(
                        "Removed zero balance for Resource {ResourceId}, UoM {UnitOfMeasurementId}",
                        receiptResource.ResourceId,
                        receiptResource.UnitOfMeasurementId);
                }
                else
                {
                    _balanceRepository.Update(balance);
                }
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation(
                "Successfully processed balance updates for deleted Receipt {ReceiptId}",
                notification.ReceiptDocumentId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "Error processing balance updates for ReceiptDocumentDeletedEvent. Receipt {ReceiptId}",
                notification.ReceiptDocumentId);
            throw;
        }
    }
}