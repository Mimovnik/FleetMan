using ErrorOr;
using FleetMan.Application.Interfaces;
using FleetMan.Application.Registration.Common;
using MediatR;


namespace FleetMan.Application.Registration.RegisterTankerShip;

public class RegisterTankerShipCommandHandler(IShipRepository repository)
        : IRequestHandler<RegisterTankerShipCommand, ErrorOr<RegisterShipResult>>
{
    private readonly IShipRepository _repository = repository;

    public async Task<ErrorOr<RegisterShipResult>> Handle(
        RegisterTankerShipCommand request,
        CancellationToken cancellationToken)
    {
        await Task.CompletedTask;
        // TODO: Implement the command handler logic
        return Error.Unexpected();
    }
}
