using System.Collections.Concurrent;
using ErrorOr;
using FleetMan.Application.Interfaces;
using FleetMan.Domain.Entities;
using FleetMan.Domain.Entities.Ships;
using FleetMan.Domain.Errors;

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

    public Task<ErrorOr<Ship>> GetByImoNumberAsync(ImoNumber imoNumber)
    {
        if (_ships.TryGetValue(imoNumber.Value, out var ship))
        {
            return Task.FromResult<ErrorOr<Ship>>(ship);
        }

        return Task.FromResult<ErrorOr<Ship>>(Errors.Ship.NotFound);
    }
}
