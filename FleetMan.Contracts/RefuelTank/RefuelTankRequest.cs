namespace FleetMan.Contracts.RefuelTank;

public record RefuelTankRequest(
    float FuelAmount,
    string FuelType
);