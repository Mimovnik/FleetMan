using ErrorOr;
using FleetMan.Application.Interfaces;
using MediatR;


namespace FleetMan.Application.RefuelTank;

public class RefuelTankCommandHandler(IShipRepository repository)
: IRequestHandler<RefuelTankCommand, ErrorOr<RefuelTankResult>>
{
    private readonly IShipRepository _repository = repository;

    public async Task<ErrorOr<RefuelTankResult>> Handle(
        RefuelTankCommand request,
        CancellationToken cancellationToken)
    {
        await Task.CompletedTask;
        // TODO: Implement the command handler logic
        return Error.Unexpected();
    }
}
