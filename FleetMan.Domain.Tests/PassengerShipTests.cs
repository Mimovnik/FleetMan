using FleetMan.Domain.Entities;
using Xunit;

namespace FleetMan.Domain.Tests;

public class PassengerShipTests
{
    [Fact]
    public void Given_ValidInput_When_CreatingPassengerShip_Then_NoError()
    {
        // Arrange
        var imoNumber = ImoNumber.Create("9074729").Value;
        var name = "Titanic";
        var length = 269.1;
        var width = 28.2;

        // Act
        var result = PassengerShip.Create(imoNumber, name, length, width);

        // Assert
        Assert.False(result.IsError);
        Assert.Equal(name, result.Value.Name);
        Assert.Equal(imoNumber, result.Value.ImoNumber);
        Assert.Empty(result.Value.Passengers);
    }

    [Theory]
    [InlineData(-123.5)]
    [InlineData(-10.0)]
    [InlineData(0.0)]
    public void Given_InvalidLength_When_CreatingPassengerShip_Then_InvalidLengthError(double invalidLength)
    {
        var imoNumber = ImoNumber.Create("9074729").Value;
        var name = "NoLengthShip";
        var width = 10.0;

        var result = PassengerShip.Create(imoNumber, name, invalidLength, width);

        Assert.True(result.IsError);
        Assert.Contains(result.Errors, e => e == Errors.Errors.Ship.InvalidLength);
    }

    [Theory]
    [InlineData(-591.0)]
    [InlineData(-1.0)]
    [InlineData(0.0)]
    public void Given_InvalidWidth_When_CreatingPassengerShip_Then_InvalidWidthError(double invalidWidth)
    {
        // Given
        var imoNumber = ImoNumber.Create("9074729").Value;
        var name = "NoWidthShip";
        var length = 50.0;

        // When
        var result = PassengerShip.Create(imoNumber, name, length, invalidWidth);

        // Then
        Assert.True(result.IsError);
        Assert.Contains(result.Errors, e => e == Errors.Errors.Ship.InvalidWidth);
    }
}
