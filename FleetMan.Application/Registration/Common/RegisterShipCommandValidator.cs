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
            .Matches(@"^\d{7}$")
                .WithErrorCode(Errors.ImoNumber.InvalidFormat.Code)
                .WithMessage(Errors.ImoNumber.InvalidFormat.Description);

        RuleFor(x => x.Length)
            .GreaterThan(0)
                .WithErrorCode(Errors.Ship.InvalidLength.Code)
                .WithMessage(Errors.Ship.InvalidLength.Description);

        RuleFor(x => x.Width)
            .GreaterThan(0)
                .WithErrorCode(Errors.Ship.InvalidWidth.Code)
                .WithMessage(Errors.Ship.InvalidWidth.Description);
    }
}
