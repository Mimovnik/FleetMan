using FleetMan.Application.Registration.RegisterPassengerShip;
using FleetMan.Contracts.Registration;
using Mapster;

namespace FleetMan.Api.Controllers;

public static class MapsterConfig
{
    public static void ConfigureMappings()
    {
        TypeAdapterConfig<RegisterShipRequest, RegisterPassengerShipCommand>
            .NewConfig()
            .Map(dest => dest.ImoNumber, src => src.ImoNumber)
            .Map(dest => dest.Name, src => src.Name)
            .Map(dest => dest.Length, src => src.Length)
            .Map(dest => dest.Width, src => src.Width);
    }
}
