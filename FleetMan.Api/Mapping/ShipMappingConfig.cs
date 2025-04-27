using FleetMan.Application.RefuelTank;
using FleetMan.Application.UpdatePassengerList;
using FleetMan.Contracts.RefuelTank;
using FleetMan.Contracts.UpdatePassengerList;
using Mapster;

namespace FleetMan.Api.Mapping;

public class ShipsMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<(string, UpdatePassengerListRequest), UpdatePassengerListCommand>()
            .Map(dest => dest.ShipImoNumber, src => src.Item1)
            .Map(dest => dest.PassengerNames, src => src.Item2.Passengers);

        config.NewConfig<(string ImoNumber, int TankNumber, RefuelTankRequest Request), RefuelTankCommand>()
            .Map(dest => dest.ImoNumber, src => src.ImoNumber)
            .Map(dest => dest.TankNumber, src => src.TankNumber)
            .Map(dest => dest.Amount, src => src.Request.FuelAmount)
            .Map(dest => dest.FuelType, src => src.Request.FuelType);
    }
}
