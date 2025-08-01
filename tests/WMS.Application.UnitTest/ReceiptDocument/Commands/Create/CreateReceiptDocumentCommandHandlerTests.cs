using Moq;
using WMS.Application.Common.Interface.Persistence;
using WMS.Application.ReceiptDocuments.Commands.Create;
using Xunit;

namespace WMS.Application.UnitTest.ReceiptDocument.Commands.Create;

public class CreateReceiptDocumentCommandHandlerTests
{
    private readonly Mock<IReceiptDocumentRepository> _receiptDocumentRepoMock = new();
    private readonly Mock<IResourceRepository> _resourceRepoMock = new();
    private readonly Mock<IUnitOfMeasurementRepository> _unitOfMeasurementRepoMock = new();
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();

    private CreateReceiptDocumentCommandHandler CreateHandler()
    {
        return new CreateReceiptDocumentCommandHandler(
            _receiptDocumentRepoMock.Object,
            _unitOfWorkMock.Object,
            _unitOfMeasurementRepoMock.Object,
            _resourceRepoMock.Object);
    }

    private CreateReceiptDocumentCommand CreateValidCommand()
    {
        return new CreateReceiptDocumentCommand
        {
            DocumentNumber = "DOC-001",
            Date = DateTime.UtcNow,
            ReceiptResources = new List<CreateReceiptDocumentCommand.ReceiptResourceCommand>
            {
                new CreateReceiptDocumentCommand.ReceiptResourceCommand
                {
                    ResourceId = Guid.NewGuid(),
                    UnitOfMeasurementId = Guid.NewGuid(),
                    Quantity = 10
                }
            }
        };
    }

    [Fact]
    public async Task Handle_DuplicateDocumentNumber_ReturnsDuplicateNumberError()
    {
        // Arrange
        var command = CreateValidCommand();
        _receiptDocumentRepoMock.Setup(r => r.ExistsByNumberAsync(command.DocumentNumber)).ReturnsAsync(true);

        var handler = CreateHandler();

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsError);
        Assert.Contains(Errors.ReceiptDocument.DuplicateNumber, result.Errors);
    }

    [Fact]
    public async Task Handle_InvalidResourceOrUom_ReturnsInvalidResourceError()
    {
        // Arrange
        var command = CreateValidCommand();

        _receiptDocumentRepoMock.Setup(r => r.ExistsByNumberAsync(command.DocumentNumber)).ReturnsAsync(false);

        // Resource exists but UoM does not exist
        _resourceRepoMock.Setup(r => r.ExistsActiveAsync(It.IsAny<Guid>())).ReturnsAsync(true);
        _unitOfMeasurementRepoMock.Setup(r => r.ExistsActiveAsync(It.IsAny<Guid>())).ReturnsAsync(false);

        var handler = CreateHandler();

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsError);
        Assert.Contains(Errors.ReceiptDocument.InvalidResource, result.Errors);
    }

    [Fact]
    public async Task Handle_InvalidQuantity_ReturnsQuantityError()
    {
        // Arrange
        var command = CreateValidCommand();
        // Inject invalid quantity (e.g., negative or zero)
        command.ReceiptResources[0].Quantity = -5;

        _receiptDocumentRepoMock.Setup(r => r.ExistsByNumberAsync(command.DocumentNumber)).ReturnsAsync(false);
        _resourceRepoMock.Setup(r => r.ExistsActiveAsync(It.IsAny<Guid>())).ReturnsAsync(true);
        _unitOfMeasurementRepoMock.Setup(r => r.ExistsActiveAsync(It.IsAny<Guid>())).ReturnsAsync(true);

        var handler = CreateHandler();

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsError);
        // Quantity.CreateNew negative should produce errors, so result.Errors should not be empty
        Assert.NotEmpty(result.Errors);
    }

    [Fact]
    public async Task Handle_ValidCommand_ReturnsReceiptDocumentResult()
    {
        // Arrange
        var command = CreateValidCommand();

        _receiptDocumentRepoMock.Setup(r => r.ExistsByNumberAsync(command.DocumentNumber)).ReturnsAsync(false);
        _resourceRepoMock.Setup(r => r.ExistsActiveAsync(It.IsAny<Guid>())).ReturnsAsync(true);
        _unitOfMeasurementRepoMock.Setup(r => r.ExistsActiveAsync(It.IsAny<Guid>())).ReturnsAsync(true);

        _receiptDocumentRepoMock.Setup(r => r.AddAsync(It.IsAny<ReceiptDocument>())).Returns(Task.CompletedTask);
        _unitOfWorkMock.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

        var handler = CreateHandler();

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result.IsError);
        Assert.NotNull(result.Value);
        Assert.Equal(command.DocumentNumber, result.Value.Number);
        Assert.Single(result.Value.ReceiptResources);
        Assert.Equal(command.ReceiptResources[0].Quantity, result.Value.ReceiptResources[0].Quantity);
    }
}