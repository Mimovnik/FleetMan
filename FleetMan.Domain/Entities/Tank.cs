using ErrorOr;

namespace FleetMan.Domain.Entities;

public class Tank
{
    public enum TankFuelType
    {
        Diesel,
        HeavyFuel,
    }

    public TankFuelType FuelType { get; }
    public float Capacity { get; }
    public float Fill { get; } = 0;

    private Tank(TankFuelType fuelType, float capacity)
    {
        FuelType = fuelType;
        Capacity = capacity;
    }

    public static ErrorOr<Tank> Create(TankFuelType fuelType, float capacity)
    {
        if (capacity <= 0)
        {
            return Errors.Errors.Tank.InvalidCapacity;
        }

        return new Tank(fuelType, capacity);
    }
}