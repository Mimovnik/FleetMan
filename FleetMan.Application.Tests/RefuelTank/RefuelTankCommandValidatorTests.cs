using FleetMan.Application.RefuelTank;
using FleetMan.Domain.Errors;
using FluentValidation.TestHelper;

namespace FleetMan.Application.Tests.RefuelTank;

public class RefuelTankCommandValidatorTests
{
    protected readonly RefuelTankCommandValidator _validator = new();

    [Fact]
    public void Validate_ValidCommand_NoErrors()
    {
        var command = new RefuelTankCommand(
            ImoNumber: "9074729",
            TankIndex: 1,
            Amount: 1000.0f,
            FuelType: "Diesel"
        );

        var result = _validator.TestValidate(command);

        result.ShouldNotHaveValidationErrorFor(x => x.ImoNumber);
        result.ShouldNotHaveValidationErrorFor(x => x.TankIndex);
        result.ShouldNotHaveValidationErrorFor(x => x.Amount);
        result.ShouldNotHaveValidationErrorFor(x => x.FuelType);
    }

    [Fact]
    public void Validate_NegativeAmount_InvalidRefuelAmountError()
    {
        var command = new RefuelTankCommand(
            ImoNumber: "1234567",
            TankIndex: 1,
            Amount: -2.0f,
            FuelType: "Diesel"
        );

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.Amount)
            .WithErrorCode(Errors.Tank.InvalidRefuelAmount.Code);
    }

    [Theory]
    [InlineData("Oil")]
    [InlineData("Empty")]
    public void Validate_InvalidFuelType_InvalidFuelTypeError(string fuelType)
    {
        var command = new RefuelTankCommand(
            ImoNumber: "1234567",
            TankIndex: 5,
            Amount: 2.0f,
            FuelType: fuelType
        );

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.FuelType)
            .WithErrorCode(Errors.Tank.InvalidFuelType.Code);
    }
}