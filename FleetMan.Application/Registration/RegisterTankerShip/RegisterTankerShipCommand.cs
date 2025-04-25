using ErrorOr;
using FleetMan.Application.DTOs;
using FleetMan.Application.Registration.Common;
using MediatR;

namespace FleetMan.Application.Registration.RegisterTankerShip;

public record RegisterTankerShipCommand(
    string ImoNumber,
    string Name,
    float Length,
    float Width,
    List<TankDto> Tanks
    ) : IRequest<ErrorOr<RegisterShipResult>>;
