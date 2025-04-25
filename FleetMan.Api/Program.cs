using FleetMan.Application;
using FleetMan.Infrastructure;

namespace FleetMan.Api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        {
            builder.Services
                .AddApi()
                .AddApplication()
                .AddInfrastructure();
        }

        var app = builder.Build();
        {
            app.MapControllers();
            app.Run();
        }
    }
}