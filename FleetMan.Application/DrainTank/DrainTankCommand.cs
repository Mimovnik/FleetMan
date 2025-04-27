using ErrorOr;
using MediatR;

namespace FleetMan.Application.DrainTank;

public record DrainTankCommand(string ImoNumber, int TankNumber) : IRequest<ErrorOr<Unit>>;