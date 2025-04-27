using ErrorOr;
using MediatR;

namespace FleetMan.Application.RefuelTank;

public record RefuelTankCommand(
    string ImoNumber,
    int TankNumber,
    float Amount,
    string FuelType
) : IRequest<ErrorOr<RefuelTankResult>>;