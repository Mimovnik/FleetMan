using FleetMan.Application.Registration.Common;
using FleetMan.Domain.Errors;
using FluentValidation;

namespace FleetMan.Application.Registration.RegisterTankerShip;

public class RegisterTankerShipCommandValidator
 : RegisterShipCommandValidator<RegisterTankerShipCommand>
{
    public RegisterTankerShipCommandValidator()
    {
        RuleFor(x => x.Tanks)
            .NotEmpty()
                .WithErrorCode(Errors.Ship.TankerShip.NoTanks.Code)
                .WithMessage(Errors.Ship.TankerShip.NoTanks.Description);

        RuleForEach(x => x.Tanks)
            .ChildRules(tank =>
            {
                tank.RuleFor(x => x.Capacity)
                    .GreaterThan(0)
                        .WithErrorCode(Errors.Tank.InvalidCapacity.Code)
                        .WithMessage(Errors.Tank.InvalidCapacity.Description);
            });
    }
}
