using ErrorOr;

namespace FleetMan.Domain.Errors;

public static class Errors
{
    public static class Ship
    {
        public static Error DuplicateImo => Error.Conflict(
            code: "Ship.DuplicateImo",
            description: "A ship with this IMO number already exists.");

        public static Error InvalidImoFormat => Error.Validation(
            code: "Ship.InvalidImoFormat",
            description: "The IMO number must be 7 characters long and contain only digits.");

        public static Error InvalidImoChecksum => Error.Validation(
            code: "Ship.InvalidImoChecksum",
            description: "The provided IMO number has invalid checksum.");

        public static Error InvalidLength => Error.Validation(
            code: "Ship.NegativeLength",
            description: "Ship length must be greater than 0.");

        public static Error InvalidWidth => Error.Validation(
            code: "Ship.NegativeWidth",
            description: "Ship width must be greater than 0.");
    }
}