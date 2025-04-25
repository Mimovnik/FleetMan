using FleetMan.Domain.Errors;
using FluentValidation;

namespace FleetMan.Application.Registration.RegisterTankerShip;

public class RegisterTankerShipCommandValidator : AbstractValidator<RegisterTankerShipCommand>
{
    public RegisterTankerShipCommandValidator()
    {
    }
}
