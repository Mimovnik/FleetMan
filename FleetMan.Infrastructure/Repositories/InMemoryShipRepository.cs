using System.Collections.Concurrent;
using FleetMan.Application.Interfaces;
using FleetMan.Domain.Entities;

namespace FleetMan.Infrastructure.Repositories;

public class InMemoryShipRepository : IShipRepository
{
    private readonly ConcurrentDictionary<string, Ship> _ships = new();

    public Task<bool> ExistsAsync(ImoNumber imoNumber)
    {
        return Task.FromResult(_ships.ContainsKey(imoNumber.Value));
    }

    public Task AddAsync(Ship ship)
    {
        _ships.TryAdd(ship.ImoNumber.Value, ship);
        return Task.CompletedTask;
    }
}
