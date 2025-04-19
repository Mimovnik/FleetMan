namespace FleetMan.Contracts.Registration;

public record RegisterShipResponse(
    string ImoNumber,
    string Name
);