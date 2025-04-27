using FleetMan.Domain.Errors;
using FluentValidation;

namespace FleetMan.Application.UpdatePassengerList;

public class UpdatePassengerListCommandValidator
: AbstractValidator<UpdatePassengerListCommand>
{
    public UpdatePassengerListCommandValidator()
    {
        RuleFor(x => x.ShipImoNumber)
            .Matches(@"^\d{7}$")
                .WithErrorCode(Errors.ImoNumber.InvalidFormat.Code)
                .WithMessage(Errors.ImoNumber.InvalidFormat.Description);

        RuleForEach(x => x.PassengerNames)
            .NotEmpty()
                .WithErrorCode(Errors.Passenger.InvalidName.Code)
                .WithMessage(Errors.Passenger.InvalidName.Description);
    }
}

