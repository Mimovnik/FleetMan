using FleetMan.Application.UpdatePassengerList;
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
    }
}
