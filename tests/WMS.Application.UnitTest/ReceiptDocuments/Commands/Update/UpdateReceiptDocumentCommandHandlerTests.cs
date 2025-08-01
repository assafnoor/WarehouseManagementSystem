using FluentAssertions;
using Moq;
using WMS.Application.Common.Interface.Persistence;
using WMS.Application.ReceiptDocuments.Commands.Create;
using WMS.Domain.Common.ErrorCatalog;
using WMS.Domain.Common.ValueObjects;
using WMS.Domain.ResourceAggregate.ValueObjects;
using WMS.Domain.UnitOfMeasurementAggregate.ValueObjects;
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
        return new CreateReceiptDocumentCommand(
            "DOC-001",
            DateTime.UtcNow,
            new List<ReceiptResourceCommand>
            {
                new ReceiptResourceCommand(
                    Guid.NewGuid(),
                    Guid.NewGuid(),
                    10.0m
                )
            }
        );
    }

    private CreateReceiptDocumentCommand CreateCommandWithoutResources()
    {
        return new CreateReceiptDocumentCommand(
            "DOC-002",
            DateTime.UtcNow,
            null
        );
    }

    private CreateReceiptDocumentCommand CreateCommandWithEmptyResources()
    {
        return new CreateReceiptDocumentCommand(
            "DOC-003",
            DateTime.UtcNow,
            new List<ReceiptResourceCommand>()
        );
    }

    [Fact]
    public async Task Handle_DuplicateDocumentNumber_ReturnsDuplicateNumberError()
    {
        // Arrange
        var command = CreateValidCommand();
        _receiptDocumentRepoMock
            .Setup(r => r.ExistsByNumberAsync(It.IsAny<DocumentNumber>()))
            .ReturnsAsync(true);

        var handler = CreateHandler();

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Should().Be(Errors.ReceiptDocument.DuplicateNumber);
    }

    [Fact]
    public async Task Handle_ResourceDoesNotExist_ReturnsInvalidResourceError()
    {
        // Arrange
        var command = CreateValidCommand();

        _receiptDocumentRepoMock
            .Setup(r => r.ExistsByNumberAsync(It.IsAny<DocumentNumber>()))
            .ReturnsAsync(false);

        // Resource does not exist
        _resourceRepoMock
            .Setup(r => r.ExistsActiveAsync(It.IsAny<ResourceId>()))
            .ReturnsAsync(false);

        _unitOfMeasurementRepoMock
            .Setup(r => r.ExistsActiveAsync(It.IsAny<UnitOfMeasurementId>()))
            .ReturnsAsync(true);

        var handler = CreateHandler();

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Should().Be(Errors.ReceiptDocument.InvalidResource);
    }

    [Fact]
    public async Task Handle_UnitOfMeasurementDoesNotExist_ReturnsInvalidResourceError()
    {
        // Arrange
        var command = CreateValidCommand();

        _receiptDocumentRepoMock
            .Setup(r => r.ExistsByNumberAsync(It.IsAny<DocumentNumber>()))
            .ReturnsAsync(false);

        // Resource exists but UoM does not exist
        _resourceRepoMock
            .Setup(r => r.ExistsActiveAsync(It.IsAny<ResourceId>()))
            .ReturnsAsync(true);

        _unitOfMeasurementRepoMock
            .Setup(r => r.ExistsActiveAsync(It.IsAny<UnitOfMeasurementId>()))
            .ReturnsAsync(false);

        var handler = CreateHandler();

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Should().Be(Errors.ReceiptDocument.InvalidResource);
    }

    [Fact]
    public async Task Handle_InvalidQuantity_ReturnsQuantityError()
    {
        // Arrange
        var command = new CreateReceiptDocumentCommand(
            "DOC-004",
            DateTime.UtcNow,
            new List<ReceiptResourceCommand>
            {
                new ReceiptResourceCommand(
                    Guid.NewGuid(),
                    Guid.NewGuid(),
                    -5.0m // Invalid negative quantity
                )
            }
        );

        _receiptDocumentRepoMock
            .Setup(r => r.ExistsByNumberAsync(It.IsAny<DocumentNumber>()))
            .ReturnsAsync(false);

        _resourceRepoMock
            .Setup(r => r.ExistsActiveAsync(It.IsAny<ResourceId>()))
            .ReturnsAsync(true);

        _unitOfMeasurementRepoMock
            .Setup(r => r.ExistsActiveAsync(It.IsAny<UnitOfMeasurementId>()))
            .ReturnsAsync(true);

        var handler = CreateHandler();

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        // The exact error depends on Quantity.CreateNew implementation
        // Usually it would be a validation error for negative quantities
    }

    [Fact]
    public async Task Handle_ZeroQuantity_ReturnsQuantityError()
    {
        // Arrange
        var command = new CreateReceiptDocumentCommand(
            "DOC-005",
            DateTime.UtcNow,
            new List<ReceiptResourceCommand>
            {
                new ReceiptResourceCommand(
                    Guid.NewGuid(),
                    Guid.NewGuid(),
                    0.0m // Invalid zero quantity
                )
            }
        );

        _receiptDocumentRepoMock
            .Setup(r => r.ExistsByNumberAsync(It.IsAny<DocumentNumber>()))
            .ReturnsAsync(false);

        _resourceRepoMock
            .Setup(r => r.ExistsActiveAsync(It.IsAny<ResourceId>()))
            .ReturnsAsync(true);

        _unitOfMeasurementRepoMock
            .Setup(r => r.ExistsActiveAsync(It.IsAny<UnitOfMeasurementId>()))
            .ReturnsAsync(true);

        var handler = CreateHandler();

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
    }

    [Fact]
    public async Task Handle_ValidCommand_ReturnsReceiptDocumentResult()
    {
        // Arrange
        var command = CreateValidCommand();

        _receiptDocumentRepoMock
            .Setup(r => r.ExistsByNumberAsync(It.IsAny<DocumentNumber>()))
            .ReturnsAsync(false);

        _resourceRepoMock
            .Setup(r => r.ExistsActiveAsync(It.IsAny<ResourceId>()))
            .ReturnsAsync(true);

        _unitOfMeasurementRepoMock
            .Setup(r => r.ExistsActiveAsync(It.IsAny<UnitOfMeasurementId>()))
            .ReturnsAsync(true);

        _receiptDocumentRepoMock
            .Setup(r => r.AddAsync(It.IsAny<WMS.Domain.ReceiptDocumentAggregate.ReceiptDocument>()))
            .Returns(Task.CompletedTask);

        _unitOfWorkMock
            .Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var handler = CreateHandler();

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Should().NotBeNull();
        result.Value.DocumentNumber.Should().Be(command.DocumentNumber);
        result.Value.ReceiptResources.Should().HaveCount(1);
        result.Value.ReceiptResources.First().Quantity.Should().Be(command.ReceiptResources![0].Quantity);
    }

    [Fact]
    public async Task Handle_ValidCommandWithoutResources_ReturnsReceiptDocumentResult()
    {
        // Arrange - Document can be empty according to business rules
        var command = CreateCommandWithoutResources();

        _receiptDocumentRepoMock
            .Setup(r => r.ExistsByNumberAsync(It.IsAny<DocumentNumber>()))
            .ReturnsAsync(false);

        _receiptDocumentRepoMock
            .Setup(r => r.AddAsync(It.IsAny<WMS.Domain.ReceiptDocumentAggregate.ReceiptDocument>()))
            .Returns(Task.CompletedTask);

        _unitOfWorkMock
            .Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var handler = CreateHandler();

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Should().NotBeNull();
        result.Value.DocumentNumber.Should().Be(command.DocumentNumber);
        result.Value.ReceiptResources.Should().BeEmpty();
    }

    [Fact]
    public async Task Handle_ValidCommandWithEmptyResources_ReturnsReceiptDocumentResult()
    {
        // Arrange - Document can be empty according to business rules
        var command = CreateCommandWithEmptyResources();

        _receiptDocumentRepoMock
            .Setup(r => r.ExistsByNumberAsync(It.IsAny<DocumentNumber>()))
            .ReturnsAsync(false);

        _receiptDocumentRepoMock
            .Setup(r => r.AddAsync(It.IsAny<WMS.Domain.ReceiptDocumentAggregate.ReceiptDocument>()))
            .Returns(Task.CompletedTask);

        _unitOfWorkMock
            .Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var handler = CreateHandler();

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Should().NotBeNull();
        result.Value.DocumentNumber.Should().Be(command.DocumentNumber);
        result.Value.ReceiptResources.Should().BeEmpty();
    }

    [Fact]
    public async Task Handle_DuplicateResourcesInCommand_ShouldMergeQuantities()
    {
        // Arrange - Multiple resources with same ResourceId and UnitOfMeasurementId should be merged
        var resourceId = Guid.NewGuid();
        var unitId = Guid.NewGuid();

        var command = new CreateReceiptDocumentCommand(
            "DOC-006",
            DateTime.UtcNow,
            new List<ReceiptResourceCommand>
            {
                new ReceiptResourceCommand(resourceId, unitId, 10.0m),
                new ReceiptResourceCommand(resourceId, unitId, 5.0m),
                new ReceiptResourceCommand(resourceId, unitId, 3.0m)
            }
        );

        _receiptDocumentRepoMock
            .Setup(r => r.ExistsByNumberAsync(It.IsAny<DocumentNumber>()))
            .ReturnsAsync(false);

        _resourceRepoMock
            .Setup(r => r.ExistsActiveAsync(It.IsAny<ResourceId>()))
            .ReturnsAsync(true);

        _unitOfMeasurementRepoMock
            .Setup(r => r.ExistsActiveAsync(It.IsAny<UnitOfMeasurementId>()))
            .ReturnsAsync(true);

        _receiptDocumentRepoMock
            .Setup(r => r.AddAsync(It.IsAny<WMS.Domain.ReceiptDocumentAggregate.ReceiptDocument>()))
            .Returns(Task.CompletedTask);

        _unitOfWorkMock
            .Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var handler = CreateHandler();

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.ReceiptResources.Should().HaveCount(1);
        result.Value.ReceiptResources.First().Quantity.Should().Be(18.0m); // 10 + 5 + 3 = 18
        result.Value.ReceiptResources.First().ResourceId.Should().Be(resourceId);
        result.Value.ReceiptResources.First().UnitOfMeasurementId.Should().Be(unitId);
    }

    [Fact]
    public async Task Handle_MultipleValidResources_ReturnsCorrectResult()
    {
        // Arrange
        var resource1Id = Guid.NewGuid();
        var resource2Id = Guid.NewGuid();
        var unit1Id = Guid.NewGuid();
        var unit2Id = Guid.NewGuid();

        var command = new CreateReceiptDocumentCommand(
            "DOC-007",
            DateTime.UtcNow,
            new List<ReceiptResourceCommand>
            {
                new ReceiptResourceCommand(resource1Id, unit1Id, 15.0m),
                new ReceiptResourceCommand(resource2Id, unit2Id, 25.0m)
            }
        );

        _receiptDocumentRepoMock
            .Setup(r => r.ExistsByNumberAsync(It.IsAny<DocumentNumber>()))
            .ReturnsAsync(false);

        _resourceRepoMock
            .Setup(r => r.ExistsActiveAsync(It.IsAny<ResourceId>()))
            .ReturnsAsync(true);

        _unitOfMeasurementRepoMock
            .Setup(r => r.ExistsActiveAsync(It.IsAny<UnitOfMeasurementId>()))
            .ReturnsAsync(true);

        _receiptDocumentRepoMock
            .Setup(r => r.AddAsync(It.IsAny<WMS.Domain.ReceiptDocumentAggregate.ReceiptDocument>()))
            .Returns(Task.CompletedTask);

        _unitOfWorkMock
            .Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var handler = CreateHandler();

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.ReceiptResources.Should().HaveCount(2);
        result.Value.ReceiptResources.Should().Contain(r => r.ResourceId == resource1Id && r.Quantity == 15.0m);
        result.Value.ReceiptResources.Should().Contain(r => r.ResourceId == resource2Id && r.Quantity == 25.0m);
    }

    [Fact]
    public async Task Handle_ValidCommand_ShouldCallRepositoryMethods()
    {
        // Arrange
        var command = CreateValidCommand();

        _receiptDocumentRepoMock
            .Setup(r => r.ExistsByNumberAsync(It.IsAny<DocumentNumber>()))
            .ReturnsAsync(false);

        _resourceRepoMock
            .Setup(r => r.ExistsActiveAsync(It.IsAny<ResourceId>()))
            .ReturnsAsync(true);

        _unitOfMeasurementRepoMock
            .Setup(r => r.ExistsActiveAsync(It.IsAny<UnitOfMeasurementId>()))
            .ReturnsAsync(true);

        _receiptDocumentRepoMock
            .Setup(r => r.AddAsync(It.IsAny<WMS.Domain.ReceiptDocumentAggregate.ReceiptDocument>()))
            .Returns(Task.CompletedTask);

        _unitOfWorkMock
            .Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var handler = CreateHandler();

        // Act
        await handler.Handle(command, CancellationToken.None);

        // Assert
        _receiptDocumentRepoMock.Verify(x => x.ExistsByNumberAsync(It.IsAny<DocumentNumber>()), Times.Once);
        _resourceRepoMock.Verify(x => x.ExistsActiveAsync(It.IsAny<ResourceId>()), Times.Once);
        _unitOfMeasurementRepoMock.Verify(x => x.ExistsActiveAsync(It.IsAny<UnitOfMeasurementId>()), Times.Once);
        _receiptDocumentRepoMock.Verify(x => x.AddAsync(It.IsAny<WMS.Domain.ReceiptDocumentAggregate.ReceiptDocument>()), Times.Once);
        _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}