using Moq;
using WMS.Application.Common.Interface.Persistence;
using WMS.Application.ReceiptDocuments.Commands.Delete;
using WMS.Domain.Common.ErrorCatalog;
using WMS.Domain.Common.ValueObjects;
using WMS.Domain.ReceiptDocumentAggregate.ValueObjects;
using WMS.Domain.ResourceAggregate.ValueObjects;
using WMS.Domain.UnitOfMeasurementAggregate.ValueObjects;
using Xunit;

namespace WMS.Application.UnitTest.ReceiptDocuments.Commands.Delete;

public class DeleteReceiptDocumentCommandHandlerTests
{
    private readonly Mock<IReceiptDocumentRepository> _receiptRepoMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;

    private readonly DeleteReceiptDocumentCommandHandler _handler;

    public DeleteReceiptDocumentCommandHandlerTests()
    {
        _receiptRepoMock = new Mock<IReceiptDocumentRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();

        _handler = new DeleteReceiptDocumentCommandHandler(
            _receiptRepoMock.Object,
            _unitOfWorkMock.Object);
    }

    [Fact]
    public async Task Handle_ReceiptDocumentExists_DeletesSuccessfully()
    {
        // Arrange
        var docId = Guid.NewGuid();
        var doc = CreateFakeReceiptDocument(docId);

        _receiptRepoMock.Setup(r => r.GetByIdAsync(ReceiptDocumentId.Create(docId)))
            .ReturnsAsync(doc);

        // Act
        var result = await _handler.Handle(new DeleteReceiptDocumentCommand(docId), CancellationToken.None);

        // Assert
        Assert.False(result.IsError);
        _receiptRepoMock.Verify(r => r.Remove(It.IsAny<WMS.Domain.ReceiptDocumentAggregate.ReceiptDocument>()), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ReceiptDocumentNotFound_ReturnsNotFound()
    {
        // Arrange
        var docId = Guid.NewGuid();
        _receiptRepoMock.Setup(r => r.GetByIdAsync(ReceiptDocumentId.Create(docId)))
            .ReturnsAsync((WMS.Domain.ReceiptDocumentAggregate.ReceiptDocument?)null);

        // Act
        var result = await _handler.Handle(new DeleteReceiptDocumentCommand(docId), CancellationToken.None);

        // Assert
        Assert.True(result.IsError);
        Assert.Equal(Errors.ReceiptDocument.NotFound, result.FirstError);
    }

    [Fact]
    public async Task Handle_ThrowsWhenBalanceInsufficient_EventHandlerFails()
    {
        // Arrange
        var docId = Guid.NewGuid();
        var doc = CreateFakeReceiptDocument(docId);

        _receiptRepoMock.Setup(r => r.GetByIdAsync(ReceiptDocumentId.Create(docId)))
            .ReturnsAsync(doc);

        _unitOfWorkMock.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ThrowsAsync(new InvalidOperationException("Insufficient balance"));

        // Act + Assert
        var ex = await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _handler.Handle(new DeleteReceiptDocumentCommand(docId), CancellationToken.None));

        Assert.Equal("Insufficient balance", ex.Message);
    }

    private WMS.Domain.ReceiptDocumentAggregate.ReceiptDocument CreateFakeReceiptDocument(Guid id)
    {
        var document = WMS.Domain.ReceiptDocumentAggregate.ReceiptDocument.Create(
            DocumentNumber.CreateNew("RC-1001"),
            DateTime.UtcNow);

        var resourceId = ResourceId.Create(Guid.NewGuid());
        var unitId = UnitOfMeasurementId.Create(Guid.NewGuid());
        document.AddResource(resourceId, unitId, Quantity.CreateNew(value: 5).Value);

        // Hack لتحديد الـ ID في الاختبار
        typeof(WMS.Domain.ReceiptDocumentAggregate.ReceiptDocument).GetProperty("Id")!.SetValue(document, ReceiptDocumentId.Create(id));

        return document;
    }
}