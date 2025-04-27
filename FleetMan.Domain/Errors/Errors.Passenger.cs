using ErrorOr;

namespace FleetMan.Domain.Errors;

public static partial class Errors
{
    public static class Passenger
    {
        public static Error InvalidName => Error.Validation(
            code: "Passenger.InvalidName",
            description: "Passenger name cannot be empty.");
    }
}