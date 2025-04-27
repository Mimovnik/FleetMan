using System.ComponentModel.DataAnnotations;

namespace FleetMan.Contracts.Registration;

public enum ShipType
{
    Passenger,
    Tanker,
}

public record Tank(
    string FuelType,
    float Capacity
);

public record RegisterShipRequest(
    [Required] ShipType? ShipType,
    string ImoNumber,
    string Name,
    float Length,
    float Width,
    IEnumerable<Tank>? Tanks = null
);