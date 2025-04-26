using ErrorOr;
using FleetMan.Application.Interfaces;
using MediatR;


namespace FleetMan.Application.UpdatePassengerList;

public class UpdatePassengerListCommandHandler(IShipRepository repository)
: IRequestHandler<UpdatePassengerListCommand, ErrorOr<Unit>>
{
    private readonly IShipRepository _repository = repository;

    public async Task<ErrorOr<Unit>> Handle(
        UpdatePassengerListCommand request,
        CancellationToken cancellationToken)
    {
        await Task.CompletedTask;

        // TODO: Implement the logic

        return Unit.Value;
    }
}
