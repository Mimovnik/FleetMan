using FleetMan.Application.Registration.Common;
using FleetMan.Domain.Errors;
using FluentValidation.TestHelper;

namespace FleetMan.Application.Tests.Registration.Common;

public abstract class RegisterShipCommandValidatorTests<TCommand, TValidator>
    where TCommand : RegisterShipCommand
    where TValidator : RegisterShipCommandValidator, new()
{
    protected readonly TValidator _validator = new();

    protected abstract TCommand CreateCommand(string imoNumber, string name, float length, float width);

    [Fact]
    public void Validate_ValidCommand_NoErrors()
    {
        var command = CreateCommand(
            imoNumber: "9074729",
            name: "Titanic",
            length: 269.1f,
            width: 28.2f
        );

        var result = _validator.TestValidate(command);

        result.ShouldNotHaveValidationErrorFor(x => x.ImoNumber);
    }

    [Theory]
    [InlineData("")]
    [InlineData("12")]
    [InlineData("123456")]
    public void Validate_ImoNumberTooShort_InvalidImoFormatError(string imo)
    {
        var command = CreateCommand(
            imoNumber: imo,
            name: "Titanic",
            length: 269.1f,
            width: 28.2f
        );

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.ImoNumber)
              .WithErrorMessage(Errors.Ship.InvalidImoFormat.Description);
    }

    [Theory]
    [InlineData("12345678")]
    [InlineData("123456789")]
    [InlineData("123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890")]
    public void Validate_ImoNumberTooLong_InvalidImoFormatError(string imo)
    {
        var command = CreateCommand(
            imoNumber: imo,
            name: "Titanic",
            length: 269.1f,
            width: 28.2f
        );

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.ImoNumber)
              .WithErrorMessage(Errors.Ship.InvalidImoFormat.Description);
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
        var command = CreateCommand(
            imoNumber: imo,
            name: "Titanic",
            length: 269.1f,
            width: 28.2f
        );

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.ImoNumber)
              .WithErrorMessage(Errors.Ship.InvalidImoFormat.Description);
    }

    [Theory]
    [InlineData("123456\u00A9")]
    [InlineData("123456\u00AE")]
    [InlineData("123456\uDEA2")]
    public void Validate_ImoNumberWithUnicode_InvalidFormatError(string imo)
    {
        var command = CreateCommand(
            imoNumber: imo,
            name: "Titanic",
            length: 269.1f,
            width: 28.2f
        );

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.ImoNumber)
              .WithErrorMessage(Errors.Ship.InvalidImoFormat.Description);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-1.12355)]
    [InlineData(-81923.5)]
    public void Validate_LengthNonPositive_InvalidLengthError(float length)
    {
        var command = CreateCommand(
            imoNumber: "9074729",
            name: "Titanic",
            length: length,
            width: 28.2f
        );

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.Length)
              .WithErrorMessage(Errors.Ship.InvalidLength.Description);
    }


    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-1.12355)]
    [InlineData(-81923.5)]
    public void Validate_Width_NonPositive_InvalidWidthError(float width)
    {
        var command = CreateCommand(
            imoNumber: "9074729",
            name: "Titanic",
            length: 269.1f,
            width: width
        );

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.Width)
              .WithErrorMessage(Errors.Ship.InvalidWidth.Description);
    }
}