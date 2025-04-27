using ErrorOr;

namespace FleetMan.Domain.Entities;

public class Tank
{
    public enum TankFuelType
    {
        Empty,
        Diesel,
        HeavyFuel,
    }

    public TankFuelType FuelType { get; private set; } = TankFuelType.Empty;
    public float Capacity { get; }
    public float Fill { get; private set; } = 0;

    public bool IsEmpty => FuelType == TankFuelType.Empty; 

    private Tank(float capacity)
    {
        Capacity = capacity;
    }

    public static ErrorOr<Tank> Create(float capacity)
    {
        if (capacity <= 0)
        {
            return Errors.Errors.Tank.InvalidCapacity;
        }

        return new Tank(capacity);
    }

    public ErrorOr<Success> RefuelWith(TankFuelType fuelType, float amount)
    {
        if (fuelType == TankFuelType.Empty)
            return Errors.Errors.Tank.InvalidFuelType;

        if (amount <= 0)
            return Errors.Errors.Tank.InvalidRefuelAmount;

        if (Fill + amount > Capacity)
            return Errors.Errors.Tank.TankWouldOverflow;
        
        bool fuelMismatch = FuelType != fuelType;
        if (!IsEmpty && fuelMismatch)
            return Errors.Errors.Tank.FuelMismatch;

        if (IsEmpty)
        {
            FuelType = fuelType;
        }

        Fill += amount;
        return Result.Success;
    }

    public void Drain()
    {
        Fill = 0;
        FuelType = TankFuelType.Empty;
    }
}