using FleetMan.Domain.Errors;
using FluentValidation;
using static FleetMan.Domain.Entities.Tank;

namespace FleetMan.Application.RefuelTank;

public class RefuelTankCommandValidator
: AbstractValidator<RefuelTankCommand>
{
    private static bool BeValidFuelType(string value)
    {
        bool parsable = Enum.TryParse(value, ignoreCase: true, out TankFuelType parsed);

        return parsable && parsed != TankFuelType.Empty;
    }

    public RefuelTankCommandValidator()
    {
        RuleFor(x => x.Amount)
            .GreaterThan(0)
                .WithErrorCode(Errors.Tank.InvalidRefuelAmount.Code)
                .WithMessage(Errors.Tank.InvalidRefuelAmount.Description);

        RuleFor(x => x.FuelType)
            .Must(BeValidFuelType)
                .WithErrorCode(Errors.Tank.InvalidFuelType.Code)
                .WithMessage(Errors.Tank.InvalidFuelType.Description);
    }
}

