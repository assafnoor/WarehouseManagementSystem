using FluentAssertions;
using Moq;
using WMS.Application.Common.Interface.Persistence;
using WMS.Application.Resources.Commands.Create;
using WMS.Domain.Common.ErrorCatalog;
using WMS.Domain.ResourceAggregate;
using Xunit;

namespace WMS.Application.UnitTest;

public class ResourceCommandHandlerTests
{
    private readonly Mock<IResourceRepository> _mockRepo;
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly ResourceCommandHandler _handler;

    public ResourceCommandHandlerTests()
    {
        _mockRepo = new Mock<IResourceRepository>();
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _handler = new ResourceCommandHandler(_mockRepo.Object, _mockUnitOfWork.Object);
    }

    [Fact]
    public async Task Handle_Should_Create_Resource_When_Valid()
    {
        // Arrange
        var command = new ResourceCommand("Valid Resource");
        _mockRepo.Setup(r => r.GetByNameAsync("Valid Resource"))
                 .ReturnsAsync((Resource?)null);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Name.Should().Be("Valid Resource");
        result.Value.IsActive.Should().BeTrue();

        _mockRepo.Verify(r => r.AddAsync(It.IsAny<Resource>()), Times.Once);
        _mockUnitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_Should_Return_Error_When_Name_Exists()
    {
        // Arrange
        var existing = Resource.Create("Duplicate Resource");
        var command = new ResourceCommand("Duplicate Resource");
        _mockRepo.Setup(r => r.GetByNameAsync("Duplicate Resource"))
                 .ReturnsAsync(existing);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Should().Be(Errors.Resource.NameAlreadyExists);
    }
}