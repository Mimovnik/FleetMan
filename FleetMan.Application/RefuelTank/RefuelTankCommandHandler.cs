using ErrorOr;
using FleetMan.Application.Interfaces;
using FleetMan.Domain.Entities;
using FleetMan.Domain.Entities.Ships;
using FleetMan.Domain.Errors;
using MediatR;
using static FleetMan.Domain.Entities.Tank;


namespace FleetMan.Application.RefuelTank;

public class RefuelTankCommandHandler(IShipRepository repository)
: IRequestHandler<RefuelTankCommand, ErrorOr<RefuelTankResult>>
{
    private readonly IShipRepository _repository = repository;

    private ErrorOr<TankFuelType> GetTankFuelType(string fuelType)
    {
        if(Enum.TryParse(fuelType, true, out TankFuelType parsed))
            return parsed;
        return Errors.Tank.InvalidFuelType;
    }

    public async Task<ErrorOr<RefuelTankResult>> Handle(
        RefuelTankCommand request,
        CancellationToken cancellationToken)
    {
        var imoResult = ImoNumber.Create(request.ImoNumber);
        if (imoResult.IsError)
            return imoResult.Errors;
        var imoNumber = imoResult.Value;

        var fuelTypeResult = GetTankFuelType(request.FuelType);
        if (fuelTypeResult.IsError)
            return fuelTypeResult.Errors;
        var fuelType = fuelTypeResult.Value;

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

        var refuelResult = tank.RefuelWith(fuelType, request.Amount);
        if (refuelResult.IsError)
            return refuelResult.Errors;

        return new RefuelTankResult(
            ShipImoNumber: tankerShip.ImoNumber.ToString(),
            TankNumber: request.TankNumber,
            FuelAmount: tank.Fill,
            FuelType: fuelType.ToString()
        );
    }
}
