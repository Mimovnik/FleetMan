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
    }
}