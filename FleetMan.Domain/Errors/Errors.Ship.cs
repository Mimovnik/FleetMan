using ErrorOr;

namespace FleetMan.Domain.Errors;

public static partial class Errors
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

        public static Error NotFound => Error.NotFound(
            code: "Ship.NotFound",
            description: "Could not find a ship with the given IMO number.");

        public static Error NotPassenger => Error.Validation(
            code: "Ship.NotPassenger",
            description: "The ship is not a passenger ship.");

        public static class TankerShip
        {
            public static Error NoTanks => Error.Validation(
                code: "TankerShip.NoTanks",
                description: "A tanker must have at least one tank.");
        }
    }
}