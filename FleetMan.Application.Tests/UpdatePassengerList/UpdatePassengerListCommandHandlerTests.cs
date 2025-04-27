using FleetMan.Application.Interfaces;
using FleetMan.Application.UpdatePassengerList;
using FleetMan.Domain.Entities;
using FleetMan.Domain.Entities.Ships;
using FleetMan.Domain.Errors;
using FluentAssertions;
using Moq;

namespace FleetMan.Application.Tests.UpdatePassengerList;

public class UpdatePassengerListCommandHandlerTests
{
    private readonly Mock<IShipRepository> _repositoryMock;
    private readonly UpdatePassengerListCommandHandler _handler;

    public UpdatePassengerListCommandHandlerTests()
    {
        _repositoryMock = new Mock<IShipRepository>();
        _handler = new UpdatePassengerListCommandHandler(_repositoryMock.Object);
    }

    [Theory]
    [InlineData("")]
    [InlineData("12345678")]
    [InlineData("-1234567")]
    [InlineData("1a34b67")]
    [InlineData("123456\u20AC")]
    public async Task Handle_CommandHasInvalidImoNumberFormat_ReturnsInvalidImoNumberError(string imo)
    {
        var command = new UpdatePassengerListCommand(imo, [
            "John Doe",
            "Jane Boe",
            "Jack Noe"
        ]);

        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsError.Should().BeTrue();
        result.Errors.Should().ContainSingle(e => e == Errors.ImoNumber.InvalidFormat);
    }
    

    [Fact]
    public async Task Handle_CommandHasEmptyPassengerNames_ReturnsInvalidNameError()
    {
        _repositoryMock
            .Setup(r => r.GetByImoNumberAsync(It.IsAny<ImoNumber>()))
            .ReturnsAsync(PassengerShip.Create(
                ImoNumber.Create("9074729").Value,
                name: "Passenger Ship",
                length: 300,
                width: 50
            ).Value);

        var command = new UpdatePassengerListCommand("9074729", [
            "",
            "Jane Boe",
            string.Empty
        ]);

        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsError.Should().BeTrue();
        result.Errors.Should().ContainSingle(e => e == Errors.Passenger.InvalidName);
    }

    [Fact]
    public async Task Handle_ShipIsOfPassengerType_ReturnsNoError()
    {
        _repositoryMock
            .Setup(r => r.GetByImoNumberAsync(It.IsAny<ImoNumber>()))
            .ReturnsAsync(PassengerShip.Create(
                ImoNumber.Create("9074729").Value,
                name: "Passenger Ship",
                length: 300,
                width: 50
            ).Value);

        var command = new UpdatePassengerListCommand("9074729", [
            "John Doe",
            "Jane Boe",
            "Jack Noe",
        ]);

        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsError.Should().BeFalse();
    }

    [Fact]
    public async Task Handle_ShipIsOfTankerType_ReturnsNotPassengerShipError()
    {
        _repositoryMock
            .Setup(r => r.GetByImoNumberAsync(It.IsAny<ImoNumber>()))
            .ReturnsAsync(TankerShip.Create(
                ImoNumber.Create("9074729").Value,
                name: "Tanker Ship",
                length: 300,
                width: 50,
                tanks: [
                    Tank.Create(Tank.TankFuelType.Diesel, 1000).Value
                ]
            ).Value);

        var command = new UpdatePassengerListCommand("9074729", [
            "John Doe",
            "Jane Boe",
            "Jack Noe",
        ]);

        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsError.Should().BeTrue();
        result.Errors.Should().ContainSingle(e => e == Errors.Ship.NotPassenger);
    }

    [Fact]
    public async Task Handle_ShipNotFound_ReturnsShipNotFoundError()
    {
        _repositoryMock
            .Setup(r => r.GetByImoNumberAsync(It.IsAny<ImoNumber>()))
            .ReturnsAsync(Errors.Ship.NotFound);

        var command = new UpdatePassengerListCommand("9074729", [
            "John Doe",
            "Jane Boe",
            "Jack Noe",
        ]);

        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsError.Should().BeTrue();
        result.Errors.Should().ContainSingle(e => e == Errors.Ship.NotFound);
    }
}