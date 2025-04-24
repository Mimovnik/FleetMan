using ErrorOr;
using FleetMan.Application.Interfaces;
using FleetMan.Application.Registration.Common;
using FleetMan.Domain.Entities;
using FleetMan.Domain.Errors;
using MediatR;


namespace FleetMan.Application.Registration.RegisterPassengerShip;
public class RegisterPassengerShipCommandHandler(IShipRepository repository)
        : IRequestHandler<RegisterPassengerShipCommand, ErrorOr<RegisterShipResult>>
{
    private readonly IShipRepository _repository = repository;

    public async Task<ErrorOr<RegisterShipResult>> Handle(
        RegisterPassengerShipCommand request,
        CancellationToken cancellationToken)
    {
        await Task.CompletedTask;

        var imoNumberResult = ImoNumber.Create(request.ImoNumber);

        if (imoNumberResult.IsError)
            return imoNumberResult.Errors;

        var imoNumber = imoNumberResult.Value;

        var shipResult = PassengerShip.Create(
            imoNumber,
            request.Name,
            request.Length,
            request.Width
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
