namespace FleetMan.Contracts.Registration;

public record RegisterPassengerShipRequest(
    string ImoNumber,
    string Name,
    float Length,
    float Width
);