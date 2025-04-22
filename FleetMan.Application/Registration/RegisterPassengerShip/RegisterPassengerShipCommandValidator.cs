using FleetMan.Domain.Errors;
using FluentValidation;

namespace FleetMan.Application.Registration.RegisterPassengerShip;

public class RegisterPassengerShipCommandValidator : AbstractValidator<RegisterPassengerShipCommand>
{
    public RegisterPassengerShipCommandValidator()
    {
        RuleFor(x => x.ImoNumber)
            .Length(7).WithMessage(Errors.Ship.InvalidImoFormat.Description)
            .Matches(@"^\d{7}$").WithMessage(Errors.Ship.InvalidImoFormat.Description);

        RuleFor(x => x.Length)
            .GreaterThan(0).WithMessage(Errors.Ship.InvalidLength.Description);

        RuleFor(x => x.Width)
            .GreaterThan(0).WithMessage(Errors.Ship.InvalidWidth.Description);
    }
}
