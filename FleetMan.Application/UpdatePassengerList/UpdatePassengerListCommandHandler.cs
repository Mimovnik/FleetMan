using ErrorOr;
using FleetMan.Application.Interfaces;
using FleetMan.Domain.Entities;
using FleetMan.Domain.Entities.Ships;
using FleetMan.Domain.Errors;
using MediatR;


namespace FleetMan.Application.UpdatePassengerList;

public class UpdatePassengerListCommandHandler(IShipRepository repository)
: IRequestHandler<UpdatePassengerListCommand, ErrorOr<Unit>>
{
    private readonly IShipRepository _repository = repository;

    private ErrorOr<List<Passenger>> CreatePassengerList(
        List<string> passengerNames)
    {
        var passengers = new List<Passenger>();

        foreach (var name in passengerNames)
        {
            var passengerResult = Passenger.Create(name);
            if (passengerResult.IsError)
                return passengerResult.Errors;

            passengers.Add(passengerResult.Value);
        }

        return passengers;
    }

    public async Task<ErrorOr<Unit>> Handle(
        UpdatePassengerListCommand request,
        CancellationToken cancellationToken)
    {
        var imoResult = ImoNumber.Create(request.ShipImoNumber);
        if (imoResult.IsError)
            return imoResult.Errors;
        var imoNumber = imoResult.Value;

        var shipResult = await _repository.GetByImoNumberAsync(imoNumber);
        if (shipResult.IsError)
            return shipResult.Errors;
        var ship = shipResult.Value;

        if (ship is not PassengerShip passengerShip)
            return Errors.Ship.NotPassenger;

        var passengerListResult = CreatePassengerList([.. request.PassengerNames]);
        if (passengerListResult.IsError)
            return passengerListResult.Errors;
        var passengerList = passengerListResult.Value;

        passengerShip.SetPassengerList(passengerList);

        return Unit.Value;
    }
}
