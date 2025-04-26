using FleetMan.Application.UpdatePassengerList;
using FleetMan.Domain.Errors;
using FluentValidation.TestHelper;

namespace FleetMan.Application.Tests.UpdatePassengerList;

public class UpdatePassengerListCommandValidatorTests
{
    protected readonly UpdatePassengerListCommandValidator _validator = new();

    [Fact]
    public void Validate_ValidCommand_NoErrors()
    {
        var command = new UpdatePassengerListCommand(
            ShipImoNumber: "9074729",
            PassengerNames: [
                "John Doe",
                "Jane Smith",
                "Alice Johnson",
                "Bob Brown",
                "Johnson Brandon"
            ]
        );

        var result = _validator.TestValidate(command);

        result.ShouldNotHaveValidationErrorFor(x => x.PassengerNames);
    }

    [Fact]
    public void Validate_PassengersNameIsEmpty_InvalidNameError()
    {
        var command = new UpdatePassengerListCommand(
            ShipImoNumber: "9074729",
            PassengerNames: [ "", string.Empty]
        );

        var result = _validator.TestValidate(command);

        // Use strings instead of lambdas
        // because FluentValidation.TestHelper can't handle indexers in lambdas
        result.ShouldHaveValidationErrorFor("PassengerNames[0]")
              .WithErrorMessage(Errors.Passenger.InvalidName.Description);

        result.ShouldHaveValidationErrorFor("PassengerNames[1]")
              .WithErrorMessage(Errors.Passenger.InvalidName.Description);
    }

    [Theory]
    [InlineData("")]
    [InlineData("12")]
    [InlineData("123456")]
    public void Validate_ImoNumberTooShort_InvalidImoFormatError(string imo)
    {
        var command = new UpdatePassengerListCommand(
            ShipImoNumber: imo,
            PassengerNames: [
                "John Doe",
                "Jane Smith",
                "Alice Johnson",
                "Bob Brown",
                "Johnson Brandon"
            ]
        );

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.ShipImoNumber)
              .WithErrorMessage(Errors.ImoNumber.InvalidFormat.Description);
    }

    [Theory]
    [InlineData("12345678")]
    [InlineData("123456789")]
    [InlineData("123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890")]
    public void Validate_ImoNumberTooLong_InvalidImoFormatError(string imo)
    {
        var command = new UpdatePassengerListCommand(
            ShipImoNumber: imo,
            PassengerNames: [
                "John Doe",
                "Jane Smith",
                "Alice Johnson",
                "Bob Brown",
                "Johnson Brandon"
            ]
        );

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.ShipImoNumber)
              .WithErrorMessage(Errors.ImoNumber.InvalidFormat.Description);
    }

    [Theory]
    [InlineData("a234567")]
    [InlineData("1b34567")]
    [InlineData("12c4567")]
    [InlineData("123d567")]
    [InlineData("1234e67")]
    [InlineData("12345f7")]
    [InlineData("123456g")]
    public void Validate_ImoNumberWithLetters_InvalidImoFormatError(string imo)
    {
        var command = new UpdatePassengerListCommand(
            ShipImoNumber: imo,
            PassengerNames: [
                "John Doe",
                "Jane Smith",
                "Alice Johnson",
                "Bob Brown",
                "Johnson Brandon"
            ]
        );

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.ShipImoNumber)
              .WithErrorMessage(Errors.ImoNumber.InvalidFormat.Description);
    }

    [Theory]
    [InlineData("123456\u00A9")]
    [InlineData("123456\u00AE")]
    [InlineData("123456\uDEA2")]
    public void Validate_ImoNumberWithUnicode_InvalidFormatError(string imo)
    {
        var command = new UpdatePassengerListCommand(
            ShipImoNumber: imo,
            PassengerNames: [
                "John Doe",
                "Jane Smith",
                "Alice Johnson",
                "Bob Brown",
                "Johnson Brandon"
            ]
        );

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.ShipImoNumber)
              .WithErrorMessage(Errors.ImoNumber.InvalidFormat.Description);
    }
}