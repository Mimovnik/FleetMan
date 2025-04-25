using FleetMan.Domain.Entities;
using FluentAssertions;

namespace FleetMan.Domain.Tests;

public class TankTests
{
    [Theory]
    [InlineData(-200)]
    [InlineData(0)]
    public void Create_NonPositiveCapacity_ReturnsInvalidCapacityError(float nonPositiveCapacity)
    {
        var result = Tank.Create(Tank.TankFuelType.Diesel, nonPositiveCapacity);
        result.IsError.Should().BeTrue();
        result.Errors.Should().ContainSingle(e => e == Errors.Errors.Tank.InvalidCapacity);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(123.45)]
    public void Create_ValidArgs_CapacityMatches(float capacity)
    {
        var result = Tank.Create(Tank.TankFuelType.HeavyFuel, capacity);
        result.IsError.Should().BeFalse();
        result.Value.Should().NotBeNull();
        result.Value.Capacity.Should().Be(capacity);
    }

    [Theory]
    [InlineData(Tank.TankFuelType.Diesel)]
    [InlineData(Tank.TankFuelType.HeavyFuel)]
    public void Create_ValidArgs_FuelTypeMatches(Tank.TankFuelType fuelType)
    {
        var result = Tank.Create(fuelType, 100);
        result.IsError.Should().BeFalse();
        result.Value.Should().NotBeNull();
        result.Value.FuelType.Should().Be(fuelType);
    }

    [Theory]
    [InlineData(12, Tank.TankFuelType.Diesel)]
    [InlineData(524.5, Tank.TankFuelType.HeavyFuel)]
    public void Create_ValidArgs_FillIsZero(float capacity, Tank.TankFuelType fuelType)
    {
        var result = Tank.Create(fuelType, capacity);
        result.IsError.Should().BeFalse();
        result.Value.Should().NotBeNull();
        result.Value.Fill.Should().Be(0);
    }
}
