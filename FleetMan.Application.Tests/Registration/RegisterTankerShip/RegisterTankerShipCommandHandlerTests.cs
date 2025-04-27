using FleetMan.Application.DTOs;
using FleetMan.Application.Interfaces;
using FleetMan.Application.Registration.RegisterTankerShip;
using FleetMan.Domain.Entities;
using FleetMan.Domain.Entities.Ships;
using FleetMan.Domain.Errors;
using FluentAssertions;
using Moq;
using static FleetMan.Domain.Entities.Tank.TankFuelType;

namespace FleetMan.Application.Tests.Registration.RegisterTankerShip;

public class RegisterTankerShipCommandHandlerTests
{
    private readonly Mock<IShipRepository> _repositoryMock;
    private readonly RegisterTankerShipCommandHandler _handler;

    public RegisterTankerShipCommandHandlerTests()
    {
        _repositoryMock = new Mock<IShipRepository>();
        _handler = new RegisterTankerShipCommandHandler(_repositoryMock.Object);
    }

    private static List<Tank> CreateValidTanks()
    {
        return [
            Tank.Create(Diesel, 1024.123f).Value,
            Tank.Create(HeavyFuel, 10).Value,
            Tank.Create(Diesel, 10000).Value,
            Tank.Create(HeavyFuel, 124.5f).Value,
        ];
    }

    [Fact]
    public async Task Handle_DuplicateImoNumbers_DuplicateImoError()
    {
        // Arrange
        var imo = ImoNumber.Create("9074729").Value;
        var existingShip = TankerShip.Create(
            imo,
            "ExistingShip",
            100,
            20,
            CreateValidTanks()).Value;

        _repositoryMock
            .Setup(r => r.ExistsAsync(It.IsAny<ImoNumber>()))
            .ReturnsAsync(true);

        var command = new RegisterTankerShipCommand("9074729", "Titanic", 300, 50, [
            new TankDto("Diesel", 1024.123f),
            new TankDto("HeavyFuel", 10),
        ]);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.Errors.Should().ContainSingle(e => e == Errors.Ship.DuplicateImo);
    }

    [Fact]
    public async Task Handle_UniqueImoNumbers_ReturnsRegisterShipResult()
    {
        _repositoryMock
            .Setup(r => r.ExistsAsync(It.IsAny<ImoNumber>()))
            .ReturnsAsync(false);

        _repositoryMock
        .Setup(r => r.AddAsync(It.IsAny<TankerShip>()))
        .Returns(Task.CompletedTask);

        var command = new RegisterTankerShipCommand("9074729", "Titanic", 300, 50, [
                new TankDto("Diesel", 1024.123f),
                new TankDto("HeavyFuel", 10),
            ]);

        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsError.Should().BeFalse();
        result.Value.Name.Should().Be("Titanic");
        result.Value.ImoNumber.Should().Be("9074729");
    }
}