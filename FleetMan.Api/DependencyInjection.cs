using FleetMan.Api.Mapping;

namespace FleetMan.Api;

public static class DependencyInjection
{
    public static IServiceCollection AddApi(this IServiceCollection services)
    {
        services.AddMappings();
        services.AddControllers();
        return services;
    }
}