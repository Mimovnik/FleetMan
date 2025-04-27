using FleetMan.Application.Interfaces;
using FleetMan.Application.RefuelTank;
using FleetMan.Application.UpdatePassengerList;
using FleetMan.Domain.Entities;
using FleetMan.Domain.Entities.Ships;
using FleetMan.Domain.Errors;
using FluentAssertions;
using Moq;

namespace FleetMan.Application.Tests.RefuelTank;

public class RefuelTankCommandHandlerTests
{
    private readonly Mock<IShipRepository> _repositoryMock;
    private readonly RefuelTankCommandHandler _handler;

    public RefuelTankCommandHandlerTests()
    {
        _repositoryMock = new Mock<IShipRepository>();
        _handler = new RefuelTankCommandHandler(_repositoryMock.Object);
    }

    private static PassengerShip CreatePassengerShip(string validImo)
    {
        return PassengerShip.Create(
            ImoNumber.Create(validImo).Value,
            "Titanic",
            269.1,
            28.2).Value;
    }

    [Fact]
    public async Task Handle_ShipNotFound_ReturnsShipNotFoundError()
    {
        _repositoryMock
            .Setup(r => r.GetByImoNumberAsync(It.IsAny<ImoNumber>()))
            .ReturnsAsync(Errors.Ship.NotFound);

        var command = new RefuelTankCommand(
            ImoNumber: "9074729",
            TankNumber: 1,
            Amount: 10,
            FuelType: "Diesel"
        );

        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsError.Should().BeTrue();
        result.Errors.Should().ContainSingle(e => e == Errors.Ship.NotFound);
    }

    [Fact]
    public async Task Handle_ShipIsNotTanker_ReturnsNotTankerError()
    {
        string validImo = "9074729";
        _repositoryMock
            .Setup(r => r.GetByImoNumberAsync(It.IsAny<ImoNumber>()))
            .ReturnsAsync(CreatePassengerShip(validImo));

        var command = new RefuelTankCommand(
            ImoNumber: validImo,
            TankNumber: 1,
            Amount: 10,
            FuelType: "Diesel"
        );

        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsError.Should().BeTrue();
        result.Errors.Should().ContainSingle(e => e == Errors.Ship.NotTanker);
    }

    [Fact]
    public async Task Handle_NonExistingTank_ReturnsTankNotFoundError()
    {
        string validImo = "9074729";
        _repositoryMock
            .Setup(r => r.GetByImoNumberAsync(It.IsAny<ImoNumber>()))
            .ReturnsAsync(
                TankerShip.Create(
                    ImoNumber.Create(validImo).Value,
                    "Titanic",
                    269.1,
                    28.2,
                    [
                        Tank.Create(1000).Value,
                        Tank.Create(2000).Value,
                        Tank.Create(2000).Value,
                        Tank.Create(2500).Value,
                    ]).Value
            );

        var command = new RefuelTankCommand(
            ImoNumber: validImo,
            TankNumber: 9,
            Amount: 10,
            FuelType: "Diesel"
        );

        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsError.Should().BeTrue();
        result.Errors.Should().ContainSingle(e => e == Errors.Tank.NotFound);
    }
}