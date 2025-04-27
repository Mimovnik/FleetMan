using ErrorOr;
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
                new TankDto(1000.24f),
                new TankDto(2000),
                new TankDto(5000)
            ]);
    }

    [Fact]
    public void Validate_TanksIsNull_TanksFieldRequiredError()
    {
        var command = new RegisterTankerShipCommand(
            ImoNumber: "9074729",
            Name: "Titanic",
            Length: 269.1f,
            Width: 28.2f,
            null
        );

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.Tanks)
              .WithErrorMessage(Errors.Ship.TankerShip.NoTanks.Description);
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

        result.ShouldHaveValidationErrorFor(x => x.Tanks)
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
                new(negativeCapacity),
                new(2000),
                new(negativeCapacity)
            ]
        );

        var result = _validator.TestValidate(command);
        // Use strings instead of lambdas
        // because FluentValidation.TestHelper can't handle indexers in lambdas
        result.ShouldHaveValidationErrorFor("Tanks[0].Capacity")
              .WithErrorMessage(Errors.Tank.InvalidCapacity.Description);

        result.ShouldNotHaveValidationErrorFor("Tanks[1].Capacity");

        result.ShouldHaveValidationErrorFor("Tanks[2].Capacity")
              .WithErrorMessage(Errors.Tank.InvalidCapacity.Description);
    }
}