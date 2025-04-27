using ErrorOr;
using FleetMan.Application.Interfaces;
using MediatR;


namespace FleetMan.Application.DrainTank;

public class DrainTankCommandHandler(IShipRepository repository)
: IRequestHandler<DrainTankCommand, ErrorOr<Unit>>
{
    private readonly IShipRepository _repository = repository;

    public async Task<ErrorOr<Unit>> Handle(
        DrainTankCommand request,
        CancellationToken cancellationToken)
    {
        await Task.CompletedTask;
        // TODO: Implement the command handler logic
        return Unit.Value;
    }
}
