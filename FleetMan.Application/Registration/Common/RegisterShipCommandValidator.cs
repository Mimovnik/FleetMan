using FleetMan.Domain.Errors;
using FluentValidation;

namespace FleetMan.Application.Registration.Common;

public abstract class RegisterShipCommandValidator<TCommand>
 : AbstractValidator<TCommand>
 where TCommand : RegisterShipCommand
{
    public RegisterShipCommandValidator()
    {
        RuleFor(x => x.ImoNumber)
            .Length(7).WithMessage(Errors.ImoNumber.InvalidFormat.Description)
            .Matches(@"^\d{7}$").WithMessage(Errors.ImoNumber.InvalidFormat.Description);

        RuleFor(x => x.Length)
            .GreaterThan(0).WithMessage(Errors.Ship.InvalidLength.Description);

        RuleFor(x => x.Width)
            .GreaterThan(0).WithMessage(Errors.Ship.InvalidWidth.Description);
    }
}
