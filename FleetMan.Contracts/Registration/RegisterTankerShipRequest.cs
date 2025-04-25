namespace FleetMan.Contracts.Registration;

public record Tank(
    string FuelType,
    float Capacity
);

public record RegisterTankerShipRequest(
    string ImoNumber,
    string Name,
    float Length,
    float Width,
    IEnumerable<Tank> Tanks
);