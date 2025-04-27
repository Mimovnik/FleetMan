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
        var result = Tank.Create(nonPositiveCapacity);
        result.IsError.Should().BeTrue();
        result.Errors.Should().ContainSingle(e => e == Errors.Errors.Tank.InvalidCapacity);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(123.45)]
    public void Create_ValidArgs_CapacityMatches(float capacity)
    {
        var result = Tank.Create(capacity);
        result.IsError.Should().BeFalse();
        result.Value.Should().NotBeNull();
        result.Value.Capacity.Should().Be(capacity);
    }

    [Theory]
    [InlineData(12)]
    [InlineData(524.5)]
    public void Create_ValidArgs_FillIsZero(float capacity)
    {
        var result = Tank.Create(capacity);
        result.IsError.Should().BeFalse();
        result.Value.Should().NotBeNull();
        result.Value.Fill.Should().Be(0);
    }

    [Fact]
    public void RefuelWith_EmptyFuelType_ReturnsInvalidFuelTypeError()
    {
        var tank = Tank.Create(100).Value;
        var result = tank.RefuelWith(Tank.TankFuelType.Empty, 10);
        result.IsError.Should().BeTrue();
        result.Errors.Should().ContainSingle(e => e == Errors.Errors.Tank.InvalidFuelType);
    }

    [Fact]
    public void RefuelWith_NonPositiveAmount_ReturnsInvalidRefuelAmountError()
    {
        var tank = Tank.Create(100).Value;
        var result = tank.RefuelWith(Tank.TankFuelType.Diesel, -10);
        result.IsError.Should().BeTrue();
        result.Errors.Should().ContainSingle(e => e == Errors.Errors.Tank.InvalidRefuelAmount);
    }

    [Fact]
    public void RefuelWith_ExceedsCapacity_ReturnsTankWouldOverflowError()
    {
        var tank = Tank.Create(100).Value;
        tank.RefuelWith(Tank.TankFuelType.Diesel, 50);
        var result = tank.RefuelWith(Tank.TankFuelType.Diesel, 60);
        result.IsError.Should().BeTrue();
        result.Errors.Should().ContainSingle(e => e == Errors.Errors.Tank.TankWouldOverflow);
    }

    [Fact]
    public void RefuelWith_FuelMismatch_ReturnsFuelMismatchError()
    {
        var tank = Tank.Create(100).Value;
        tank.RefuelWith(Tank.TankFuelType.Diesel, 50);
        var result = tank.RefuelWith(Tank.TankFuelType.HeavyFuel, 20);
        result.IsError.Should().BeTrue();
        result.Errors.Should().ContainSingle(e => e == Errors.Errors.Tank.FuelMismatch);
    }

    [Fact]
    public void RefuelWith_ValidArgs_UpdatesFill()
    {
        var tank = Tank.Create(100).Value;
        var result = tank.RefuelWith(Tank.TankFuelType.Diesel, 50);
        result.IsError.Should().BeFalse();
        tank.Fill.Should().Be(50);
    }

    [Fact]
    public void RefuelWith_ValidArgs_UpdatesFuelType()
    {
        var tank = Tank.Create(100).Value;
        var result = tank.RefuelWith(Tank.TankFuelType.Diesel, 50);
        result.IsError.Should().BeFalse();
        tank.FuelType.Should().Be(Tank.TankFuelType.Diesel);
    }

    [Fact]
    public void Drain_ResetsFill()
    {
        var tank = Tank.Create(100).Value;
        tank.RefuelWith(Tank.TankFuelType.Diesel, 50);
        tank.Drain();
        tank.Fill.Should().Be(0);
    }

    [Fact]
    public void Drain_ResetsFuelType()
    {
        var tank = Tank.Create(100).Value;
        tank.RefuelWith(Tank.TankFuelType.Diesel, 50);
        tank.Drain();
        tank.FuelType.Should().Be(Tank.TankFuelType.Empty);
    }
}
