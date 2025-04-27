using ErrorOr;

namespace FleetMan.Domain.Errors;

public static partial class Errors
{
    public static class ImoNumber
    {
        public static Error InvalidFormat => Error.Validation(
            code: "ImoNumber.InvalidFormat",
            description: "The IMO number must be 7 characters long and contain only digits.");

        public static Error InvalidChecksum => Error.Validation(
            code: "ImoNumber.InvalidChecksum",
            description: "The provided IMO number has invalid checksum.");
    }
}