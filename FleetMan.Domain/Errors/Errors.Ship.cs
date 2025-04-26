using ErrorOr;

namespace FleetMan.Domain.Errors;

public static class Errors
{
    public static class Ship
    {
        public static Error DuplicateImo => Error.Conflict(
            code: "Ship.DuplicateImo",
            description: "A ship with this IMO number already exists.");

        public static Error InvalidLength => Error.Validation(
            code: "Ship.NegativeLength",
            description: "Ship length must be greater than 0.");

        public static Error InvalidWidth => Error.Validation(
            code: "Ship.NegativeWidth",
            description: "Ship width must be greater than 0.");

        public static class TankerShip
        {
            public static Error NoTanks => Error.Validation(
                code: "TankerShip.NoTanks",
                description: "A tanker must have at least one tank.");
        }
    }

    public static class ImoNumber
    {
        public static Error InvalidFormat => Error.Validation(
            code: "ImoNumber.InvalidFormat",
            description: "The IMO number must be 7 characters long and contain only digits.");

        public static Error InvalidChecksum => Error.Validation(
            code: "ImoNumber.InvalidChecksum",
            description: "The provided IMO number has invalid checksum.");
    }

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