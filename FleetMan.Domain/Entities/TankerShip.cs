using ErrorOr;

namespace FleetMan.Domain.Entities;

public class TankerShip : Ship
{
    public List<Tank> Tanks { get; }

    private TankerShip(ImoNumber imoNumber,
                       string name,
                       double length,
                       double width,
                       List<Tank> tanks)
        : base(imoNumber, name, length, width)
    {
        Tanks = tanks;
    }

    public static ErrorOr<TankerShip> Create(
        ImoNumber imoNumber,
        string name,
        double length,
        double width,
        IEnumerable<Tank> tanks)
    {
        var errors = new List<Error>();

        if (length <= 0)
            errors.Add(Errors.Errors.Ship.InvalidLength);

        if (width <= 0)
            errors.Add(Errors.Errors.Ship.InvalidWidth);

        if (!tanks.Any())
            errors.Add(Errors.Errors.Ship.TankerShip.NoTanks);

        if (errors.Count != 0)
            return errors;

        return new TankerShip(imoNumber, name, length, width, [.. tanks]);
    }
}