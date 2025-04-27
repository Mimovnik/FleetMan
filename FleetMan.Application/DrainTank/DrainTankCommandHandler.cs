using ErrorOr;
using FleetMan.Application.Interfaces;
using FleetMan.Domain.Entities;
using FleetMan.Domain.Entities.Ships;
using FleetMan.Domain.Errors;
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
        var imoResult = ImoNumber.Create(request.ImoNumber);
        if (imoResult.IsError)
            return imoResult.Errors;
        var imoNumber = imoResult.Value;

        var shipResult = await _repository.GetByImoNumberAsync(imoNumber);
        if (shipResult.IsError)
            return shipResult.Errors;
        var ship = shipResult.Value;

        if (ship is not TankerShip tankerShip)
            return Errors.Ship.NotTanker;
        
        var tankIndex = request.TankNumber - 1;
        if (tankIndex < 0 || tankIndex >= tankerShip.Tanks.Count)
            return Errors.Tank.NotFound;

        var tank = tankerShip.Tanks[tankIndex];

        tank.Drain();

        return Unit.Value;
    }
}
