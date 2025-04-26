using FleetMan.Domain.Entities;
using FluentAssertions;

namespace FleetMan.Domain.Tests;

public class PassengerTests
{
    [Fact]
    public void Create_WithValidName_ReturnsPassenger()
    {
        string name = "John Doe";

        var result = Passenger.Create(name);

        result.IsError.Should().BeFalse();
        result.Should().NotBeNull();
        result.Value.Name.Should().Be(name);
    }

    [Fact]
    public void Create_WithNullName_InvalidNameError()
    {
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
        var result = Passenger.Create(null);
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.

        result.IsError.Should().BeTrue();
        result.Errors.Should().ContainSingle(e => e == Errors.Errors.Passenger.InvalidName);
    }

    [Fact]
    public void Create_WithEmptyName_InvalidNameError()
    {
        string name = string.Empty;

        var result = Passenger.Create(name);

        result.IsError.Should().BeTrue();
        result.Errors.Should().ContainSingle(e => e == Errors.Errors.Passenger.InvalidName);
    }
}