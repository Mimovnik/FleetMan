using FleetMan.Application.Registration.Common;
using FleetMan.Domain.Errors;
using FluentValidation;

namespace FleetMan.Application.Registration.RegisterTankerShip;

public class RegisterTankerShipCommandValidator
 : RegisterShipCommandValidator<RegisterTankerShipCommand>
{
    public RegisterTankerShipCommandValidator()
    {
        RuleFor(x => x.ImoNumber)
            .Length(7).WithMessage(Errors.Ship.InvalidImoFormat.Description)
            .Matches(@"^\d{7}$").WithMessage(Errors.Ship.InvalidImoFormat.Description);

        RuleFor(x => x.Length)
            .GreaterThan(0).WithMessage(Errors.Ship.InvalidLength.Description);

        RuleFor(x => x.Width)
            .GreaterThan(0).WithMessage(Errors.Ship.InvalidWidth.Description);

        RuleFor(x => x.Tanks)
            .NotEmpty().WithMessage(Errors.Ship.TankerShip.NoTanks.Description);

        RuleForEach(x => x.Tanks)
            .ChildRules(tank =>
            {
                tank.RuleFor(x => x.Capacity)
                    .GreaterThan(0).WithMessage(Errors.Tank.InvalidCapacity.Description);
            });
    }
}
