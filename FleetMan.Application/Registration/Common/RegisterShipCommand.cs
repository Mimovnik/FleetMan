using ErrorOr;
using MediatR;

namespace FleetMan.Application.Registration.Common;

public abstract record RegisterShipCommand(
    string ImoNumber,
    string Name,
    float Length,
    float Width) : IRequest<ErrorOr<RegisterShipResult>>;
