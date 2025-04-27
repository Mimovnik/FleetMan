namespace FleetMan.Application.RefuelTank;

public record RefuelTankResult(
    string ShipImoNumber,
    int TankNumber,
    float FuelAmount,
    string FuelType
);