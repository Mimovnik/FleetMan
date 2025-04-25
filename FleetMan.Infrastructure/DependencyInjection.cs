using FleetMan.Application.Interfaces;
using FleetMan.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace FleetMan.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddSingleton<IShipRepository, InMemoryShipRepository>();

        return services;
    }
}
