using System.Text.Json.Serialization;
using FleetMan.Api.Mapping;

namespace FleetMan.Api;

public static class DependencyInjection
{
    public static IServiceCollection AddApi(this IServiceCollection services)
    {
        services.AddMappings();
        services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.UnmappedMemberHandling =
                        JsonUnmappedMemberHandling.Disallow;
                    options.JsonSerializerOptions.Converters
                        .Add(new JsonStringEnumConverter());
                });
        return services;
    }
}