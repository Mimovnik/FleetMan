using ErrorOr;

namespace FleetMan.Domain.Entities;

public class Passenger
{
    public string Name { get; }

    private Passenger(string name)
    {
        Name = name;
    }

    public static ErrorOr<Passenger> Create(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return Errors.Errors.Passenger.InvalidName;

        return new Passenger(name);
    }
}