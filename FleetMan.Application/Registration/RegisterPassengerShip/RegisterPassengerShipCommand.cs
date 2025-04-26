using FleetMan.Application.Registration.Common;

namespace FleetMan.Application.Registration.RegisterPassengerShip;

public record RegisterPassengerShipCommand(
    string ImoNumber,
    string Name,
    float Length,
    float Width
) : RegisterShipCommand(ImoNumber, Name, Length, Width);
