using ErrorOr;
using FleetMan.Application.DTOs;
using FleetMan.Application.Interfaces;
using FleetMan.Application.Registration.Common;
using FleetMan.Domain.Entities;
using FleetMan.Domain.Errors;
using MediatR;


namespace FleetMan.Application.Registration.RegisterTankerShip;

public class RegisterTankerShipCommandHandler(IShipRepository repository)
        : IRequestHandler<RegisterTankerShipCommand, ErrorOr<RegisterShipResult>>
{
    private readonly IShipRepository _repository = repository;

    private static ErrorOr<List<Tank>> CreateTanks(List<TankDto> tankDtos)
    {
        var tankList = new List<Tank>();

        foreach (var tank in tankDtos)
        {
            bool fuelTypeIsCorrect = Enum.TryParse(tank.FuelType, out Tank.TankFuelType fuelTypeResult);

            if (!fuelTypeIsCorrect)
                return Errors.Tank.InvalidFuelType;

            var tankResult = Tank.Create(fuelTypeResult, tank.Capacity);

            if (tankResult.IsError)
                return tankResult.Errors;

            tankList.Add(tankResult.Value);
        }

        return tankList;
    }

    public async Task<ErrorOr<RegisterShipResult>> Handle(
        RegisterTankerShipCommand request,
        CancellationToken cancellationToken)
    {
        var imoNumberResult = ImoNumber.Create(request.ImoNumber);
        if (imoNumberResult.IsError)
            return imoNumberResult.Errors;
        var imoNumber = imoNumberResult.Value;

        if (request.Tanks == null || request.Tanks.Count == 0)
            return Errors.Ship.TankerShip.NoTanks;

        var tanksResult = CreateTanks(request.Tanks);
        if (tanksResult.IsError)
            return tanksResult.Errors;
        var tanks = tanksResult.Value;

        var shipResult = TankerShip.Create(
            imoNumber,
            request.Name,
            request.Length,
            request.Width,
            tanks
        );

        if (shipResult.IsError)
            return shipResult.Errors;

        var ship = shipResult.Value;

        if (await _repository.ExistsAsync(imoNumber))
            return Errors.Ship.DuplicateImo;

        await _repository.AddAsync(ship);

        return new RegisterShipResult(ship.ImoNumber.ToString(), ship.Name);
    }
}
