using FleetMan.Api;
using FleetMan.Application;
using FleetMan.Infrastructure;

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


