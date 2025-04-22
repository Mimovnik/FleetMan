using FleetMan.Application.Registration.RegisterPassengerShip;
using FleetMan.Domain.Errors;
using FluentValidation.TestHelper;

namespace FleetMan.Application.Tests.Registration;

public class RegisterPassengerShipCommandValidatorTests
{
    private readonly RegisterPassengerShipCommandValidator _validator = new();

    [Fact]
    public void Should_Not_Have_Error_When_ImoNumber_Is_Valid()
    {
        // Arrange
        var command = new RegisterPassengerShipCommand(
            ImoNumber: "9074729",
            Name: "Titanic",
            Length: 269.1f,
            Width: 28.2f
        );

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.ImoNumber);
    }

    [Fact]
    public void Should_Have_Error_When_ImoNumber_Is_Less_Than_7_Characters_Long()
    {
        // Arrange
        var command = new RegisterPassengerShipCommand(
            ImoNumber: "123456",
            Name: "Titanic",
            Length: 269.1f,
            Width: 28.2f
        );

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.ImoNumber)
              .WithErrorMessage(Errors.Ship.InvalidImoFormat.Description);
    }

    [Fact]
    public void Should_Have_Error_When_ImoNumber_Is_More_Than_7_Characters_Long()
    {
        // Arrange
        var command = new RegisterPassengerShipCommand(
            ImoNumber: "12345678",
            Name: "Titanic",
            Length: 269.1f,
            Width: 28.2f
        );

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.ImoNumber)
              .WithErrorMessage(Errors.Ship.InvalidImoFormat.Description);
    }

    [Fact]
    public void Should_Have_Error_When_ImoNumber_Contains_ASCII_Character(){
        // Arrange
        var command = new RegisterPassengerShipCommand(
            ImoNumber: "12a4567",
            Name: "Titanic",
            Length: 269.1f,
            Width: 28.2f
        );

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.ImoNumber)
              .WithErrorMessage(Errors.Ship.InvalidImoFormat.Description);
    }

    [Fact]
    public void Should_Have_Error_When_ImoNumber_Contains_Unicode_Character (){
        // Arrange
        var command = new RegisterPassengerShipCommand(
            ImoNumber: "123456\u00A9",
            Name: "Titanic",
            Length: 269.1f,
            Width: 28.2f
        );

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.ImoNumber)
              .WithErrorMessage(Errors.Ship.InvalidImoFormat.Description);
    }

    [Fact]
    public void Should_Have_Error_When_ImoNumber_Is_Empty()
    {
        // Arrange
        var command = new RegisterPassengerShipCommand(
            ImoNumber: "",
            Name: "Titanic",
            Length: 269.1f,
            Width: 28.2f
        );

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.ImoNumber)
              .WithErrorMessage(Errors.Ship.InvalidImoFormat.Description);
    }

    [Fact]
    public void Should_Have_Error_When_Length_Zero()
    {
        // Arrange
        var command = new RegisterPassengerShipCommand(
            ImoNumber: "9074729",
            Name: "Titanic",
            Length: 0f,
            Width: 28.2f
        );

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Length)
              .WithErrorMessage(Errors.Ship.InvalidLength.Description);
    }

    [Fact]
    public void Should_Have_Error_When_Length_Negative()
    {
        // Arrange
        var command = new RegisterPassengerShipCommand(
            ImoNumber: "9074729",
            Name: "Titanic",
            Length: -123.4f,
            Width: 28.2f
        );

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Length)
              .WithErrorMessage(Errors.Ship.InvalidLength.Description);
    }

    [Fact]
    public void Should_Have_Error_When_Width_Zero()
    {
        // Arrange
        var command = new RegisterPassengerShipCommand(
            ImoNumber: "9074729",
            Name: "Titanic",
            Length: 269.1f,
            Width: 0f
        );

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Width)
              .WithErrorMessage(Errors.Ship.InvalidWidth.Description);
    }

    [Fact]
    public void Should_Have_Error_When_Width_Negative()
    {
        // Arrange
        var command = new RegisterPassengerShipCommand(
            ImoNumber: "9074729",
            Name: "Titanic",
            Length: 269.1f,
            Width: -28.2f
        );

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Width)
              .WithErrorMessage(Errors.Ship.InvalidWidth.Description);
    }
}