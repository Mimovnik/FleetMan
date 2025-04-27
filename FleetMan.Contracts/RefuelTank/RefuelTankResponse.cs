namespace FleetMan.Contracts.RefuelTank;

public record RefuelTankResponse(
    string ShipImoNumber,
    int TankNumber,
    float FuelAmount,
    string FuelType
);