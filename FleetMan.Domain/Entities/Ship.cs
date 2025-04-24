namespace FleetMan.Domain.Entities;

public abstract class Ship
{
    public ImoNumber ImoNumber { get; }
    public string Name { get; }
    public double Length { get; }
    public double Width { get; }

    protected Ship(ImoNumber imoNumber, string name, double length, double width)
    {
        ImoNumber = imoNumber;
        Name = name;
        Length = length;
        Width = width;
    }
}
