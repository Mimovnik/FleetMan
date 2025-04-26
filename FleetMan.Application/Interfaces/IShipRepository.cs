using ErrorOr;
using FleetMan.Domain.Entities;

namespace FleetMan.Application.Interfaces;

public interface IShipRepository
{
    Task<bool> ExistsAsync(ImoNumber imoNumber);
    Task AddAsync(Ship ship);
    Task<ErrorOr<Ship>> GetByImoNumberAsync(ImoNumber imoNumber);
}