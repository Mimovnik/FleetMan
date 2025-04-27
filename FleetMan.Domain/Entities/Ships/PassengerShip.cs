using ErrorOr;

namespace FleetMan.Domain.Entities.Ships;

public class PassengerShip : Ship
{
    public List<Passenger> Passengers { get; }

    private PassengerShip(ImoNumber imoNumber, string name, double length, double width, List<Passenger> passengers)
        : base(imoNumber, name, length, width)
    {
        Passengers = passengers;
    }

    public void SetPassengerList(List<Passenger> passengers)
    {
        Passengers.Clear();
        Passengers.AddRange(passengers);
    }

    public static ErrorOr<PassengerShip> Create(
        ImoNumber imoNumber,
        string name,
        double length,
        double width)
    {
        var errors = new List<Error>();

        if (length <= 0)
            errors.Add(Errors.Errors.Ship.InvalidLength);

        if (width <= 0)
            errors.Add(Errors.Errors.Ship.InvalidWidth);

        if (errors.Count != 0)
            return errors;

        return new PassengerShip(imoNumber, name, length, width, []);
    }
}