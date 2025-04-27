using FleetMan.Application.DTOs;
using FleetMan.Application.Registration.Common;

namespace FleetMan.Application.Registration.RegisterTankerShip;

public record RegisterTankerShipCommand(
    string ImoNumber,
    string Name,
    float Length,
    float Width,
    List<TankDto>? Tanks
    ) : RegisterShipCommand(
            ImoNumber: ImoNumber,
            Name: Name,
            Length: Length,
            Width: Width
        );
