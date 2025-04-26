using FleetMan.Application.DTOs;
using FleetMan.Application.Registration.RegisterTankerShip;
using FleetMan.Application.Tests.Registration.Common;
using FleetMan.Domain.Errors;
using FluentValidation.TestHelper;

namespace FleetMan.Application.Tests.Registration.RegisterTankerShip;

public class RegisterTankerShipCommandValidatorTests
 : RegisterShipCommandValidatorTests<RegisterTankerShipCommand,
                                     RegisterTankerShipCommandValidator>
{
    protected override RegisterTankerShipCommand CreateCommand(string imoNumber, string name, float length, float width)
    {
        return new RegisterTankerShipCommand(
            imoNumber,
            name,
            length,
            width,
            Tanks:
            [
                new TankDto("Diesel", 1000.24f),
                new TankDto("HeavyFuel", 2000),
                new TankDto("Diesel", 5000)
            ]);
    }

    [Fact]
    public void Validate_NoTanks_NoTanksError()
    {
        var command = new RegisterTankerShipCommand(
            ImoNumber: "9074729",
            Name: "Titanic",
            Length: 269.1f,
            Width: 28.2f,
            Tanks: []
        );

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x =>
                    ((RegisterTankerShipCommand)x).Tanks)
              .WithErrorMessage(Errors.Ship.TankerShip.NoTanks.Description);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-591824.1985f)]
    public void Validate_NegativeTankCapacity_TankInvalidCapacityError(float negativeCapacity)
    {
        var command = new RegisterTankerShipCommand(
            ImoNumber: "9074729",
            Name: "Titanic",
            Length: 269.1f,
            Width: 28.2f,
            Tanks:
            [
                new("Diesel", negativeCapacity),
                new("HeavyFuel", 2000),
                new("HeavyFuel", negativeCapacity)
            ]
        );

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x =>
                    ((RegisterTankerShipCommand)x).Tanks
                    .All(t => t.Capacity == negativeCapacity))
              .WithErrorMessage(Errors.Tank.InvalidCapacity.Description);
    }
}