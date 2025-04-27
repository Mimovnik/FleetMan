using FleetMan.Application.Interfaces;
using FleetMan.Application.Registration.RegisterPassengerShip;
using FleetMan.Domain.Entities;
using FleetMan.Domain.Entities.Ships;
using FleetMan.Domain.Errors;
using FluentAssertions;
using Moq;

namespace FleetMan.Application.Tests.Registration.RegisterPassengerShip;

public class RegisterPassengerShipCommandHandlerTests
{
    private readonly Mock<IShipRepository> _repositoryMock;
    private readonly RegisterPassengerShipCommandHandler _handler;

    public RegisterPassengerShipCommandHandlerTests()
    {
        _repositoryMock = new Mock<IShipRepository>();
        _handler = new RegisterPassengerShipCommandHandler(_repositoryMock.Object);
    }

    [Fact]
    public async Task Given_DuplicateImoNumbers_When_Handle_Then_DuplicateImoError()
    {
        // Arrange
        var imo = ImoNumber.Create("9074729").Value;
        var existingShip = PassengerShip.Create(imo, "ExistingShip", 100, 20).Value;

        _repositoryMock
            .Setup(r => r.ExistsAsync(It.IsAny<ImoNumber>()))
            .ReturnsAsync(true);

        var command = new RegisterPassengerShipCommand("9074729", "Titanic", 300, 50);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.Errors.Should().ContainSingle(e => e == Errors.Ship.DuplicateImo);
    }

    [Fact]
    public async Task Given_NoDuplicateImoNumbers_When_Handle_Then_ReturnsRegisterShipResult()
    {
        _repositoryMock
            .Setup(r => r.ExistsAsync(It.IsAny<ImoNumber>()))
            .ReturnsAsync(false);

        _repositoryMock
        .Setup(r => r.AddAsync(It.IsAny<PassengerShip>()))
        .Returns(Task.CompletedTask);

        var command = new RegisterPassengerShipCommand("9074729", "Titanic", 300, 50);

        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsError.Should().BeFalse();
        result.Value.Name.Should().Be("Titanic");
        result.Value.ImoNumber.Should().Be("9074729");
    }
}