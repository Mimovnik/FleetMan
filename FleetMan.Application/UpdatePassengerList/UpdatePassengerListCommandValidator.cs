using FleetMan.Domain.Errors;
using FluentValidation;

namespace FleetMan.Application.UpdatePassengerList;

public class UpdatePassengerListCommandValidator
: AbstractValidator<UpdatePassengerListCommand>
{
    public UpdatePassengerListCommandValidator()
    {
        RuleFor(x => x.ShipImoNumber)
            .Length(7).WithMessage(Errors.ImoNumber.InvalidFormat.Description)
            .Matches(@"^\d{7}$").WithMessage(Errors.ImoNumber.InvalidFormat.Description);

        RuleForEach(x => x.PassengerNames)
            .NotEmpty().WithMessage(Errors.Passenger.InvalidName.Description);
    }
}

