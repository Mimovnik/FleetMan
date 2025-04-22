using ErrorOr;
using FleetMan.Application.Registration.Common;
using MediatR;

namespace FleetMan.Application.Registration.RegisterPassengerShip;

public record RegisterPassengerShipCommand(
    string ImoNumber,
    string Name,
    float Length,
    float Width) : IRequest<ErrorOr<RegisterShipResult>>;
