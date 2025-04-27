using ErrorOr;

namespace FleetMan.Domain.Errors;

public static partial class Errors
{
    public static class Tank
    {
        public static Error InvalidCapacity => Error.Validation(
            code: "Tank.NegativeCapacity",
            description: "Tank capacity must be greater than 0.");

        public static Error InvalidFuelType => Error.Validation(
            code: "Tank.InvalidFuelType",
            description: "Fuel type must be one of: 'Diesel', 'HeavyFuel'.");

        public static Error NotFound => Error.NotFound(
            code: "Tank.NotFound",
            description: "Tank not found.");

        public static Error InvalidRefuelAmount => Error.Validation(
            code: "Tank.InvalidRefuelAmount",
            description: "Refuel amount must be greater than 0.");

        public static Error FuelMismatch => Error.Validation(
            code: "Tank.FuelMismatch",
            description: "Fuel type mismatch. Cannot refuel with a different fuel type.");
        
        public static Error TankWouldOverflow => Error.Validation(
            code: "Tank.TankWouldOverflow",
            description: "Refuel would cause overflow. Cannot refuel with this amount.");
    }
}