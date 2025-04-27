using ErrorOr;
using FleetMan.Domain.Entities;
using FleetMan.Domain.Entities.Ships;
using FluentAssertions;

namespace FleetMan.Domain.Tests.ShipTests;

public class TankerShipTests
{
    private static ErrorOr<TankerShip> CreateValidTankerShip()
    {
        return TankerShip.Create(
            ImoNumber.Create("0000000").Value,
            "Titanic",
            269.1,
            28.2,
            [
                Tank.Create(1000).Value,
                Tank.Create(2000).Value,
                Tank.Create(2000).Value,
                Tank.Create(2000).Value,
            ]);
    }

    [Fact]
    public void Create_ValidArgs_ResultHasNoErrors()
    {
        var result = CreateValidTankerShip();
        result.IsError.Should().BeFalse();
    }

    [Fact]
    public void Create_ValidArgs_ResultIsNotNull()
    {
        var result = CreateValidTankerShip();
        result.Value.Should().NotBeNull();
    }

    [Fact]
    public void Create_ValidArgs_TankListIsNotNull()
    {
        var result = CreateValidTankerShip();
        result.Value.Should().NotBeNull();
        result.Value.Tanks.Should().NotBeNull();
    }

    [Fact]
    public void Create_ValidArgs_TankListIsNotEmpty()
    {
        var result = CreateValidTankerShip();
        result.Value.Should().NotBeNull();
        result.Value.Tanks.Should().NotBeEmpty();
    }

    [Theory]
    [InlineData("9074729")] // checksum = 139
    [InlineData("7601231")] // checksum = 101
    [InlineData("1234567")] // checksum = 77
    [InlineData("1000007")] // checksum = 7
    [InlineData("0100006")] // checksum = 6
    [InlineData("0010005")] // checksum = 5
    [InlineData("0001004")] // checksum = 4
    [InlineData("0000103")] // checksum = 3
    [InlineData("0000012")] // checksum = 2
    [InlineData("0000000")] // checksum = 0
    public void Create_ValidArgs_ImoNumberMatches(string imo)
    {
        var imoNumber = ImoNumber.Create(imo).Value;
        var name = "Titanic";
        var length = 269.1;
        var width = 28.2;
        var tanks = new List<Tank>
        {
            Tank.Create(1000).Value,
            Tank.Create(2000).Value,
            Tank.Create(2000).Value,
            Tank.Create(2000).Value,
        };

        var result = TankerShip.Create(imoNumber, name, length, width, tanks);

        result.Value.ImoNumber.Should().Be(imoNumber);
    }

    [Theory]
    [InlineData("")]
    [InlineData("XYZ1234")]
    [InlineData("Black Pearl")]
    [InlineData("Titanic")]
    public void Create_ValidArgs_NameMatches(string name)
    {
        var imoNumber = ImoNumber.Create("0000000").Value;
        var length = 269.1;
        var width = 28.2;
        var tanks = new List<Tank>
        {
            Tank.Create(1000).Value,
            Tank.Create(2000).Value,
            Tank.Create(2000).Value,
            Tank.Create(2000).Value,
        };

        var result = TankerShip.Create(imoNumber, name, length, width, tanks);

        result.Value.Name.Should().Be(name);
    }

    [Theory]
    [InlineData(123.5)]
    [InlineData(10.0)]
    [InlineData(50)]
    public void Create_ValidArgs_LengthMatches(float length)
    {
        var imoNumber = ImoNumber.Create("0000000").Value;
        var name = "Titanic";
        var width = 28.2;
        var tanks = new List<Tank>
        {
            Tank.Create(1000).Value,
            Tank.Create(2000).Value,
            Tank.Create(2000).Value,
            Tank.Create(2000).Value,
        };

        var result = TankerShip.Create(imoNumber, name, length, width, tanks);

        result.Value.Length.Should().Be(length);
    }

    [Theory]
    [InlineData(123.5)]
    [InlineData(10.0)]
    [InlineData(50)]
    public void Create_ValidArgs_WidthMatches(float width)
    {
        var imoNumber = ImoNumber.Create("0000000").Value;
        var name = "Titanic";
        var length = 28.2;
        var tanks = new List<Tank>
        {
            Tank.Create(1000).Value,
            Tank.Create(2000).Value,
            Tank.Create(2000).Value,
            Tank.Create(2000).Value,
        };

        var result = TankerShip.Create(imoNumber, name, length, width, tanks);

        result.Value.Width.Should().Be(width);
    }

    [Fact]
    public void Create_ValidArgs_TankCountMatches()
    {
        var imoNumber = ImoNumber.Create("0000000").Value;
        var name = "Titanic";
        var length = 128.2;
        var width = 28.2;
        var tanks = new List<Tank>
        {
            Tank.Create(1000).Value,
            Tank.Create(2000).Value,
            Tank.Create(2000).Value,
            Tank.Create(2000).Value,
        };

        var result = TankerShip.Create(imoNumber, name, length, width, tanks);

        result.Value.Tanks.Should().HaveCount(tanks.Count);
    }

    [Theory]
    [InlineData(-123.5)]
    [InlineData(-10.0)]
    [InlineData(0.0)]
    public void Create_InvalidLength_ReturnsInvalidLengthError(double invalidLength)
    {
        var imoNumber = ImoNumber.Create("9074729").Value;
        var name = "NoLengthShip";
        var width = 10.0;

        var result = PassengerShip.Create(imoNumber, name, invalidLength, width);

        result.IsError.Should().BeTrue();
        result.Errors.Should().ContainSingle(e => e == Errors.Errors.Ship.InvalidLength);
    }

    [Theory]
    [InlineData(-591.0)]
    [InlineData(-1.0)]
    [InlineData(0.0)]
    public void Create_InvalidWidth_ReturnsInvalidWidthError(double invalidWidth)
    {
        var imoNumber = ImoNumber.Create("9074729").Value;
        var name = "NoWidthShip";
        var length = 50.0;

        var result = PassengerShip.Create(imoNumber, name, length, invalidWidth);

        result.IsError.Should().BeTrue();
        result.Errors.Should().ContainSingle(e => e == Errors.Errors.Ship.InvalidWidth);
    }
}
